
namespace MyFinanceTracker.Api.Models
{
    public class User
    {
        internal DateTime CreatedAt;

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty; 
        public string PasswordHash { get; set; } = string.Empty; // storing hashed passwords only
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public string? GoogleId { get; internal set; }
    }
}
