namespace MyFinanceTracker.Api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; } // Could be an enum in a more complex app
        public int UserId { get; set; } // Foreign key to the User model
                                        
        // Navigation property for EF Core
        public User User { get; set; }
    }

    //public class BankAccount : Account
    //{
    //    public string InstitutionName { get; set; }
    //    public string AccountNumber { get; set; }
    //    public double? InterestRate { get; set; } // Nullable for accounts that don't have an interest rate
    //}

}
