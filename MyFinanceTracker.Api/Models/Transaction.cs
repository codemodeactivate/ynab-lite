namespace MyFinanceTracker.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string? Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
