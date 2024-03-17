namespace MyFinanceTracker.Api.Models
{
    public abstract class BankAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; } // user should be able to rename, delete
        public string AccountNumber { get; set; } 
        public string BankName { get; set; }    
        public decimal Balance { get; set; } // balance should be updated with each transaction
        public int UserId { get; set; } // foreign key

        //Navigation property
        public User User { get; set; }
    }

    public class CheckingAccount : BankAccount
    {
        public string AccountType { get; set; } // user should be to change type
    }

    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; } 
    }
}
