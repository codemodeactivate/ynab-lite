using MyFinanceTracker.Api.Models;

namespace MyFinanceTracker.Api.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Make sure the database is created
            context.Database.EnsureCreated();

            // Check if there are any transactions. If yes, the DB has been seeded
            if (context.Transactions.Any())
            {
                return; // DB has been seeded
            }

            // Create sample tags
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

            // Create sample categories
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

            // Create sample transactions
            var transactions = new Transaction[]
            {
            new Transaction{Description="Walmart Groceries", Amount=150.00M, Date=DateTime.Now, Memo="Weekly groceries", IsCleared=true, CategoryId=1, Tags=new List<Tag>{tags[0]}},
            new Transaction{Description="Electric Bill", Amount=75.00M, Date=DateTime.Now.AddMonths(-1), Memo="July bill", IsCleared=true, CategoryId=2, Tags=new List<Tag>{tags[1]}}
            };
            foreach (Transaction t in transactions)
            {
                context.Transactions.Add(t);
            }
            context.SaveChanges();
        }
    }
}
