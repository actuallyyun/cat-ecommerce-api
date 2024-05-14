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
            return await _categoryService.CreateCategoryAsync(categoryCreate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<bool> UpdateCategory(
            [FromRoute] Guid id,
            CategoryUpdateDto categoryUpdate
        )
        {
            return await _categoryService.UpdateCategoryAsync(id, categoryUpdate);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> RetrieveCategoryByIdAsync(
            [FromRoute] Guid id
        )
        {
            return await _categoryService.GetCategoryByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<CategoryReadDto>> ListAllCategories()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategory([FromRoute] Guid id)
        {
            return await _categoryService.DeleteCategoryAsync(id);
        }
    }
}
