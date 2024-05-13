using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepositoryAbstraction
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Guid id, Category category);
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<bool> DeleteCategoryAsync(Guid categoryId);
        Task<Category> FindByNameAsync(string name);
    }

}