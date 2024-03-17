using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Api.Data;
using MyFinanceTracker.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;    

namespace MyFinanceTracker.Api.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        //{
        //    var transactions = await _context.Transactions.ToListAsync();
        //    if (transactions == null || !transactions.Any())
        //    {
        //        return NotFound("No transactions found."); // Or return an empty list to indicate no data
        //    }

        //    // For debugging: Log the count of transactions
        //    Console.WriteLine($"Found {transactions.Count} transactions.");

        //    return Ok(transactions); // Return raw transactions for initial testing
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> Get()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Tags)
                .Include(t => t.Category) // Include the category for access to Name
                .ToListAsync();

            var TransactionDtos = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date,
                Memo = t.Memo,
                IsCleared = t.IsCleared,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Tags = t.Tags.Select(tag => tag.Name).ToList()
            });

            return Ok(TransactionDtos);


        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Post(TransactionDto transactionDto)
        {
            // Assuming BankAccountId is non-nullable but checking for existence
            var bankAccount = await _context.BankAccounts.FindAsync(transactionDto.BankAccountId);
            if (bankAccount == null)
            {
                return BadRequest($"Bank account not found.");
            }

            var transaction = new Transaction
            {
                Description = transactionDto.Description,
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Memo = transactionDto.Memo,
                IsCleared = transactionDto.IsCleared,
                CategoryId = transactionDto.CategoryId,
                BankAccountId = transactionDto.BankAccountId, // Directly assigned, as it's non-nullable
            };

            bankAccount.Balance += transaction.Amount;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var responseDto = new TransactionDto
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Memo = transaction.Memo,
                IsCleared = transaction.IsCleared,
                CategoryId = transaction.CategoryId,
                BankAccountId = transaction.BankAccountId, // Assuming BankAccountId is a simple int
                                                           // Populate other required fields like CategoryName and Tags if necessary
            };

            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, responseDto);
        }


    }
}