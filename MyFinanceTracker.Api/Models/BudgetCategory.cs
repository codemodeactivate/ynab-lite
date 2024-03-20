using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceTracker.Api.Models
{
    public class BudgetCategory
    {
        public int BudgetCategoryId { get; set; }
        [ForeignKey("Budget")]
        public int BudgetId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal SpentAmount { get; set; } = 0; // Optionally track spending

        // Navigation properties
        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }
    }

}
