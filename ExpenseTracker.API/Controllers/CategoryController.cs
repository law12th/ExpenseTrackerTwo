using ExpenseTracker.Domain.Models;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers {
    [Route(RouteConstants.CategoriesController)]
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

        [HttpGet]
        [Route(RouteConstants.CategoryByKey + "{id}")]
        public async Task<IActionResult> ReadCategoryByKey(int id) {
            try {
                if (id <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var category = await context.Categories.FindAsync(id);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("category/create")]
        public async Task<IActionResult> CreateCategory(Category category) {
            try {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsCategoryDuplicate(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Categories.Add(category);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetCategoryByKey", new { id = category.CategoryID }, category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("category/update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category) {
            try {
                if (id != category.CategoryID)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsCategoryDuplicate(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Entry(category).State = EntityState.Modified;
                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("category/delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {
            try {
                var category = await context.Categories.FindAsync(id);

                if (category == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (await IsCategoryInUse(category))
                    return StatusCode(StatusCodes.Status400BadRequest);

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<bool> IsCategoryDuplicate(Category category) {
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

        private async Task<bool> IsCategoryInUse(Category category) {
            try {
                var asset = await context.Expenses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.CategoryID == category.CategoryID);

                if (asset != null)
                    return true;

                return false;
            }
            catch {
                throw;
            }
        }
    }
}
