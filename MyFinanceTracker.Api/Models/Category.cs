namespace MyFinanceTracker.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Optional: Additional properties for budgeting, etc.
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
