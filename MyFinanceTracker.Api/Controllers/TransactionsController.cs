using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyFinanceTracker.Api.Controllers
{
    [ApiController] // This attribute tells ASP.NET Core that this class is an API controller
    [Route("[controller]")] // This attribute tells ASP.NET Core that this controller should be accessed via the /transactions route
    public class TransactionsController : ControllerBase 
    {
        private static List<Transaction> transactions = new List<Transaction>();
        private static int nextId = 1;

        //GET /transactions
        [HttpGet]
        public IEnumerable<Transaction> Get() // This method will be called when a GET request is made to /transactions
            //IEnumberable<Transaction> is a collection of Transaction objects  
        {
            return transactions;
        }

        //POST /transactions
        [HttpPost]
        public IActionResult Post(Transaction transaction)
        {
            transaction.Id = nextId++;
            transactions.Add(transaction);
            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction); 
        }


    }
}