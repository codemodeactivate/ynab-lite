﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MyFinanceTracker.Api.Data;
using MyFinanceTracker.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Api.Models;
using BCrypt.Net;




namespace MyFinanceTracker.Api.Controllers



{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto)
        {
            //check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("User already exists with this email.");
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
            // include the user object in res body

        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Temporary check for testing with plain text password
            bool passwordIsValid = user != null &&
                (user.PasswordHash == loginDto.Password ||
                 BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash));

            if (!passwordIsValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
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

    }
}