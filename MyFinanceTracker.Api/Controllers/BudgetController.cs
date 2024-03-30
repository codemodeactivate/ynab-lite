using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Api.Data;
using MyFinanceTracker.Api.DTOs;
using MyFinanceTracker.Api.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MyFinanceTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _context;


        public BudgetController(AppDbContext context)
        {
            _context = context;
        }


        //POST api/Budget
        [HttpPost]
        public async Task<ActionResult<Budget>> CreateBudget([FromBody] BudgetDto budgetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var budget = new Budget
            {
                UserId = userId,
                BudgetName = budgetDto.BudgetName,
                StartDate = budgetDto.StartDate,
                EndDate = budgetDto.EndDate
            };

            _context.Budgets.Add(budget);

            // Default Categories

            var defaultCategoryNames = new[]
            {
                "Bills",
                "Credit Card Payments",
                "Needs",
                "Wants",
                "Wishes"
            };

            foreach (var categoryName in defaultCategoryNames)
            {
                var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName /* && Correct condition here */);

                if (category == null)
                {
                    category = new Category { Name = categoryName /* Assign UserId if needed */ };
                    _context.Categories.Add(category);
                    // Note: No need to call SaveChangesAsync here; it's called later to save all changes.
                }
                // The category.Id check for 0 is unnecessary if you're using EF Core's automatic Id generation (e.g., with [DatabaseGenerated(DatabaseGeneratedOption.Identity)] attribute).
                budget.BudgetCategories.Add(new BudgetCategory { Category = category });

            }

            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetBudgetById), new { id = budget.BudgetId }, budget);

        }

        [HttpGet("{id}", Name = nameof(GetBudgetById))]
        public async Task<ActionResult<Budget>> GetBudgetById(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            return Ok(budget);
        }

    }
}