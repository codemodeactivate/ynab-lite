using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Api.Models;


namespace MyFinanceTracker.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>() // TPH configuration
                .HasDiscriminator<string>("AccountType") // Discriminator column
                .HasValue<CheckingAccount>("Checking")
                .HasValue<SavingsAccount>("Savings");
        }
    }
}