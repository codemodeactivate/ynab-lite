using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceTracker.Api.Models
{
    [Table("Budgets")]
    public class Budget
    {
        public int BudgetId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string BudgetName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<BudgetCategory> BudgetCategories { get; set; } = new List<BudgetCategory>();
    }

}
