using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult<Category>> CreateCategoryAsync(
            CategoryCreateDto categoryCreate
        )
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(categoryCreate);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("id")]
        public async Task<bool> UpdateCategory(
            [FromRoute] Guid id,
            CategoryUpdateDto categoryUpdate
        )
        {
            return await _categoryService.UpdateCategoryAsync(id, categoryUpdate);
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<ActionResult<CategoryReadDto>> RetrieveCategoryByIdAsync(
            [FromRoute] Guid id
        )
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> ListAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
