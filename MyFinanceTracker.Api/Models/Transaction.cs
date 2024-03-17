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

        //category reference
        public int CategoryId { get; set; }
        public Category Category { get; set; } 
    }

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
        public List<string> Tags { get; set; } = new List<string>();
    }

}
