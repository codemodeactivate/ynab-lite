using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyFinanceTracker.Api.Data;
using MyFinanceTracker.Api.DTOs;
using MyFinanceTracker.Api.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyFinanceTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountsController> _logger;
        private readonly IConfiguration _configuration;

        public AccountsController(AppDbContext context, ILogger<AccountsController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("debug/token")]
        public IActionResult DebugToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                _logger.LogInformation("Manual token validation succeeded.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Manual token validation failed: {ex.Message}");
                return Unauthorized($"Token validation failed: {ex.Message}");
            }

            return Ok("Token is valid.");
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDto accountDto)
        {
            _logger.LogInformation("Attempting to create a new account.");

            if (accountDto == null)
            {
                _logger.LogWarning("CreateAccount called with null account data.");
                return BadRequest("Account data cannot be null.");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogWarning("Failed to extract User ID from token. User ID claim is missing.");
                return Unauthorized("Authentication failed. User ID is missing from the token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            BankAccount account;
            switch (accountDto.AccountType.ToLower()) // Convert to lowercase for case-insensitivity
            {
                case "checking":
                    account = new CheckingAccount();
                    break;
                case "savings":
                    account = new SavingsAccount();
                    {
                        if (accountDto.InterestRate.HasValue)
                        {
                            ((SavingsAccount)account).InterestRate = accountDto.InterestRate.Value;
                        }
                    };
                    break;
                default:
                    _logger.LogError($"Invalid account type: {accountDto.AccountType}");
                    return BadRequest($"Invalid account type: {accountDto.AccountType}");
            }

            account.AccountName = accountDto.AccountName;
            account.AccountNumber = accountDto.AccountNumber;
            account.BankName = accountDto.InstitutionName;
            account.Balance = accountDto.Balance;
            account.UserId = userId;

            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Account successfully created with Id: {account.Id} for UserId: {userId}");

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
    }
}
