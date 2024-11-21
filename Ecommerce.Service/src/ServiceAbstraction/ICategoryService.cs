
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<bool> UpdateCategoryAsync(Guid id, CategoryUpdateDto categoryDto);
        Task<CategoryReadDto> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        Task<IEnumerable<ProductReadDto>>GetProductsByCategoryAsync(Guid id );
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
