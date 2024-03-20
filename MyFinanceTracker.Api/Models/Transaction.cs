using System.ComponentModel;

namespace MyFinanceTracker.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Memo { get; set; } = string.Empty;
        public bool IsCleared { get; set; } = false;

        public List<Tag> Tags { get; set; } = new List<Tag>();
        public int? BankAccountId { get; set; } //foreign key to bank account
        public BankAccount BankAccount { get; set; } //navigation property
        public bool IsDeposit { get; set; } // True for deposit, false for withdrawal

        //category reference
        public int CategoryId { get; set; }
        public Category Category { get; set; } 
    }

    

}
