using MyFinanceTracker.Api.Models;

namespace MyFinanceTracker.Api.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Make sure the database is created
            context.Database.EnsureCreated();

            // Check if the DB has been seeded
            if (context.Transactions.Any())
            {
                return; // DB has been seeded
            }

            //Seed Users First

            var users = new User[]
            {
                new User{Email="user1@example.com", PasswordHash="hashedPassword1", FirstName="John", LastName="Doe"},
                new User{Email="user2@example.com", PasswordHash="hashedPassword2", FirstName="Jane", LastName="Smith"}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges(); // save users first so everything else goes somewhere - who/what/when! 

            // Seed BankAccounts
            var bankAccounts = new BankAccount[]
            {
                new CheckingAccount{AccountName="Checking Account", AccountNumber="123456", BankName="Bank A", Balance=1000.00M},
                new SavingsAccount{AccountName="Savings Account", AccountNumber="654321", BankName="Bank B", Balance=5000.00M, InterestRate=1.5M}
            };
            foreach (BankAccount b in bankAccounts)
            {
                context.BankAccounts.Add(b);
            }
            context.SaveChanges();

            // Seed Tags
            var tags = new Tag[]
            {
                new Tag{Name="Groceries"},
                new Tag{Name="Utilities"}
            };
            foreach (Tag t in tags)
            {
                context.Tags.Add(t);
            }
            context.SaveChanges();

            // Seed Categories
            var categories = new Category[]
            {
                new Category{Name="Food"},
                new Category{Name="Bills"}
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            // Now, when creating Transactions, ensure the CategoryId and BankAccountId references exist.
            // Since we've just seeded Categories and BankAccounts, use actual Ids from them.
            var transactions = new Transaction[]
            {
                new Transaction{Description="Walmart Groceries", Amount=150.00M, Date=DateTime.Now, Memo="Weekly groceries", IsCleared=true, CategoryId=categories[0].Id, BankAccountId=bankAccounts[0].Id, Tags=new List<Tag>{tags[0]}},
                new Transaction{Description="Electric Bill", Amount=75.00M, Date=DateTime.Now.AddMonths(-1), Memo="July bill", IsCleared=true, CategoryId=categories[1].Id, BankAccountId=bankAccounts[1].Id, Tags=new List<Tag>{tags[1]}}
            };
            foreach (Transaction t in transactions)
            {
                context.Transactions.Add(t);
            }
            context.SaveChanges();
        }
    }
}
