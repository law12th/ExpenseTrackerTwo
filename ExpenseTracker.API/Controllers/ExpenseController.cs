using ExpenseTracker.Domain.Models;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers {
    [Route(RouteConstants.Controller)]
    [ApiController]
    public class ExpenseController : ControllerBase {
        private readonly ExpenseTrackerContext context;

        public ExpenseController(ExpenseTrackerContext context) {
            this.context = context;
        }

        [HttpGet]
        [Route(RouteConstants.Expenses)]
        public async Task<IActionResult> ReadExpenses() {
            try {
                var expenses = await context.Expenses
                    .AsNoTracking()
                    .OrderBy(c => c.ExpenseDate)
                    .ToListAsync();

                return Ok(expenses);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ExpensesByKey + "{id}")]
        public async Task<IActionResult> ReadCategoryByKey(int id) { }

        [HttpPost]
        [Route("expense/create")]
        public async Task<IActionResult> CreateExpense(Expense expense) { }

        [HttpPut]
        [Route("expense/update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense) { }

        [HttpDelete]
        [Route("category/delete/{id}")]
        public async Task<IActionResult> DeleteExpense(int id) { }

        }
}
