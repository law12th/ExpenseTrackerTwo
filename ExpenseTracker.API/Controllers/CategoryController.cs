using ExpenseTracker.Domain.Models;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly ExpenseTrackerContext context;

        public CategoryController(ExpenseTrackerContext context) { 
            this.context = context;
        }

        [HttpGet]
        [Route(RouteConstants.Categories)]
        public async Task<IActionResult> ReadCategories() {
            try {
                var categories = await context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.CategoryName)
                    .ToListAsync();

                return Ok(categories);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<bool> isCategoryDuplicate(Category category) {
            try {
                var categoryInDb = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower());

                if (categoryInDb == null)
                    return true;

                return false;
            } catch {
                throw;
            }
        }
    }
}
