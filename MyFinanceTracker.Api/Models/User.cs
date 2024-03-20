namespace MyFinanceTracker.Api.Models
{
    public class User
    {
        internal DateTime CreatedAt;

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Storing hashed passwords only
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        // Adding virtual ICollection for lazy loading and better change tracking
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

        public string? GoogleId { get; internal set; }
    }
}
