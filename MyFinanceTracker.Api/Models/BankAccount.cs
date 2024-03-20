using MyFinanceTracker.Api.Models;

public abstract class BankAccount
{
    public int Id { get; set; }
    public string AccountName { get; set; } // Renamed from AccountName for consistency
    public string AccountNumber { get; set; }
    public string BankName { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }
    public string AccountType { get; set; } // To distinguish between Checking and Savings in the base class

    // Navigation property for EF Core
    public User User { get; set; }
}

public class CheckingAccount : BankAccount
{
    // CheckingAccount specific properties can go here
    // For now, we'll inherit everything from BankAccount
}

public class SavingsAccount : BankAccount
{
    public decimal InterestRate { get; set; }
}
