using MyFinanceTracker.Api.Models;

public abstract class BankAccount
{
    public BankAccount() { }  // Ensure there's a parameterless constructor, even protected

    public int Id { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string BankName { get; set; }
    public decimal Balance { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
}

public class CheckingAccount : BankAccount
{
    public CheckingAccount() : base() { }  // Parameterless constructor for deserialization

    public string AccountType { get; set; }
}

public class SavingsAccount : BankAccount
{
    public SavingsAccount() : base() { }  // Parameterless constructor for deserialization

    public decimal InterestRate { get; set; }
}
