
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyFinanceTracker.Api.Data;
using MyFinanceTracker.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Api.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using BCrypt.Net;




namespace MyFinanceTracker.Api.Controllers



{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto)
        {
            //check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("User already exists with this email.");
            }

            //check if passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            //Hash it
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            //create new User
            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            // Add the new user to the context & save changes
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return the new user
            // for security maybe don't return everything
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            // return 201 status code, include a Location header in res pointing to where
            // newly created user can be accessed based on URL of app 


        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Login attempt for {loginDto.Email}");

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {loginDto.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Invalid password attempt for user: {loginDto.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                _logger.LogInformation($"User authenticated: {loginDto.Email}");
                var token = GenerateJwtToken(user);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during login for {loginDto.Email}");
                return StatusCode(500, "An internal server error has occurred.");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var keyBytes = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Email)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        public class GoogleAuthTokenDto
        {
            public string Token { get; set; }
        }

        [HttpPost("google-auth")]
        public async Task<IActionResult> GoogleAuth([FromBody] GoogleAuthTokenDto googleTokenDto)
        {
            string googleToken = googleTokenDto.Token;

            if (string.IsNullOrEmpty(googleToken))
            {
                return BadRequest("The Google token cannot be empty.");
            }

            try
            {
                var googleUser = await ValidateGoogleToken(googleToken);

                if (googleUser == null)
                {
                    return BadRequest("Invalid Google token. The token could not be validated.");
                }

                var email = googleUser.FindFirst(ClaimTypes.Email)?.Value;
                var userId = googleUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId))
                {
                    return BadRequest("The Google token is valid but did not return the necessary user information (email or userId).");
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        Email = email,
                        GoogleId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    existingUser = newUser;
                }
                var token = GenerateJwtToken(existingUser);
                return Ok(new { Token = token, Message = "Google authentication successful." });

            }
            catch (HttpRequestException httpEx)
            {
                // This assumes ValidateGoogleToken makes an HTTP request and could throw HttpRequestException
                _logger.LogError(httpEx, "An HTTP error occurred while validating the Google token.");
                return StatusCode(503, "A service error occurred while validating the Google token. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Google authentication request.");
                return StatusCode(500, "An internal error occurred while processing the Google authentication request.");
            }
        }


        private async Task<ClaimsPrincipal> ValidateGoogleToken(string token)
        {
            try
            {
                _logger.LogInformation("Received request for Google authentication.");

                var googleClientId = _configuration["Authentication:Google:ClientId"];
                _logger.LogInformation($"Google Client ID: {googleClientId}");

                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={token}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to validate Google token. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonSerializer.Deserialize<GoogleTokenInfo>(content);

                if (tokenInfo == null)
                {
                    _logger.LogError("Failed to deserialize token information.");
                    return null;
                }

                if (!(tokenInfo.iss == "https://accounts.google.com" || tokenInfo.iss == "accounts.google.com"))
                {
                    _logger.LogError("Invalid token issuer.");
                    return null;
                }


                // Token is valid, create ClaimsPrincipal
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, tokenInfo.sub),
            new Claim(ClaimTypes.Email, tokenInfo.email),
            // Add additional claims as needed
        };

                return new ClaimsPrincipal(new ClaimsIdentity(claims, GoogleDefaults.AuthenticationScheme));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Google authentication request.");
                return null;
            }
        }


        public class GoogleTokenInfo
        {
            public string aud { get; set; } // Audience
            public string iss { get; set; } // Issuer
            public string sub { get; set; } // Subject (user ID)
            public string email { get; set; } // User email
                                              // Add other properties as needed
        }

    }
}

