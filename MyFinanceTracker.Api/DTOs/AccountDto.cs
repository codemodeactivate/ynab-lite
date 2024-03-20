using MyFinanceTracker.Api.Models;

namespace MyFinanceTracker.Api.DTOs
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; } // Use enum here?
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public decimal? InterestRate { get; set; } // Nullable for accounts that don't have an interest rate
        public string InstitutionName { get; set; }
        public string AccountNumber { get; set; } // Consider storing/returning a masked version for security
        public DateTime CreatedAt { get; set; }

    }
}
