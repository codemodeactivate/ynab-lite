using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceTracker.Api.Models
{
    // Assuming Account is not needed as a separate entity and BankAccount is the base class.
    [Table("Accounts")] // Ensuring the table name is plural to follow conventions
    public abstract class BankAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        // Removed AccountType from here since it'll be handled by the discriminator in TPH
    }

    public class CheckingAccount : BankAccount
    {
        // Properties specific to CheckingAccount can be added here
    }

    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; }
    }
}
