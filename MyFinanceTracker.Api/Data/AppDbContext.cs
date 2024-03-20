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
        // Removed the redundant DbSet<Account> Accounts line
        public DbSet<BankAccount> BankAccounts { get; set; } // Use this for all bank account operations
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Budget>().ToTable("Budgets");


            // Configure the discriminator for the BankAccount inheritance
            modelBuilder.Entity<BankAccount>()
                .HasDiscriminator<string>("AccountType") // Discriminator column
                .HasValue<CheckingAccount>("Checking")
                .HasValue<SavingsAccount>("Savings"); // This is the end of this configuration block, so it ends with a semicolon
        }


    }
}
