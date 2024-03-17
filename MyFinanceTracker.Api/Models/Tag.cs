namespace MyFinanceTracker.Api.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Optional: Consider adding a description or color property for UI purposes
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
