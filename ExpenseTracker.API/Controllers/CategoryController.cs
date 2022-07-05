using ExpenseTracker.Domain.Models;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers {
    [Route(RouteConstants.Controller)]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly ExpenseTrackerContext context;

        public CategoryController(ExpenseTrackerContext context) { 
            this.context = context;
        }

        /// <summary>
        /// http://localhost:5239/api/asset-category/categories/
        /// </summary> 
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

        /// <summary>
        /// URL: http://localhost:5239/api/asset-category/category/key/{key}
        /// </summary>
        /// <param name="id">Primary key of the entity.</param> 
        [HttpGet]
        [Route(RouteConstants.CategoryByKey + "{key}")]
        public async Task<IActionResult> ReadCategoryByKey(int key) {
            try {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var category = await context.Categories.FindAsync(key);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return Ok(category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// URL: http://localhost:6600/api/asset-category/category/create
        /// </summary>
        /// <param name="category">Category object.</param>
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

        /// <summary>
        /// URL: http://localhost:5239/api/asset-category/category/update/key
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <param name="category">Category object.</param>
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

        /// <summary>
        /// URL: http://localhost:5239/api/asset-category/category/delete/key
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
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

        /// <summary>
        /// Checks if the category name is a duplicate or not.
        /// </summary>
        /// <param name="category">Category object.</param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Checks if a category is in use or not.
        /// </summary>
        /// <param name="category">Category object.</param>
        /// <returns>bool</returns>
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
