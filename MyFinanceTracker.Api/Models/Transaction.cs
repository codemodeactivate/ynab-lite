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
        public bool isCleared { get; set; } = false;
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
