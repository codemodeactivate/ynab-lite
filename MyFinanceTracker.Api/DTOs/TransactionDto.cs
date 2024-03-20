using MyFinanceTracker.Api.Models;

namespace MyFinanceTracker.Api.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
        public bool IsCleared { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? BankAccountId { get; set; } //foreign key to bank account
        public BankAccount? BankAccount { get; set; } //navigation property
        public bool IsDeposit { get; set; } // True for deposit, false for withdrawal
        public List<string> Tags { get; set; } = new List<string>();
    }
}
