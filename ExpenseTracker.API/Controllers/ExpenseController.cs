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

        /// <summary>
        /// http://localhost:5239/api/expense-tracker/expenses/
        /// </summary>
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

        /// <summary>
        /// URL: http://localhost:5239/api/expense-tracker/expenses/{key}
        /// </summary>
        /// <param name="key">Primary key of the entity.</param> 
        [HttpGet]
        [Route(RouteConstants.ExpensesByKey + "{key}")]
        public async Task<IActionResult> ReadExpenseByKey(int key) {
            try {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var expense = await context.Expenses.FindAsync(key);

                if (expense == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(expense);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: http://localhost:5239/api/expense-tracker/expenses/create
        /// </summary>
        /// <param name="expense">Expense object.</param>
        [HttpPost]
        [Route("expenses/create")]
        public async Task<IActionResult> CreateExpense(Expense expense) {
            try {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (isExpenseDateInFuture(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (isExpenseAmountLessThanZero(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Expenses.Add(expense);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadExpenseByKey", new { id = expense.ExpenseID }, expense);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: http://localhost:5239/api/expense-tracker/expenses/update/{key}
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <param name="expense">Expense object.</param>
        [HttpPut]
        [Route("expense/update/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, Expense expense) {
            try {
                if (id != expense.ExpenseID)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (isExpenseDateInFuture(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (isExpenseAmountLessThanZero(expense))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Entry(expense).State = EntityState.Modified;
                context.Expenses.Update(expense);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: http://localhost:5239/api/expense-tracker/expenses/delete/{key}
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        [HttpDelete]
        [Route("expenses/delete/{id}")]
        public async Task<IActionResult> DeleteExpense(int id) {
            try {
                var expense = await context.Expenses.FindAsync(id);

                if (expense == null)
                    return StatusCode(StatusCodes.Status400BadRequest);                

                context.Expenses.Remove(expense);
                await context.SaveChangesAsync();

                return Ok(expense);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Checks if the expense date is in the future or not.
        /// </summary>
        /// <param name="expense">Expense object.</param>
        /// <returns>bool</returns>
        private bool isExpenseDateInFuture(Expense expense) {
                return expense.ExpenseDate > DateTime.Now;
        }

        /// <summary>
        /// Checks if the expense amount is less than zero or not.
        /// </summary>
        /// <param name="expense">Expense object.</param>
        /// <returns>bool</returns>
        private bool isExpenseAmountLessThanZero(Expense expense) {                
                return expense.Amount <= 0;
        }
    }    
}
