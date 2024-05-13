using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class CategoryRepo : ICategoryRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepo(EcommerceDbContext context)
        {
            _context = context;
            _categories = _context.Categories;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            await _categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _categories.SingleOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, Category category)
        {
            await _categories
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Name, category.Name)
                    .SetProperty(c => c.Image, category.Image)
                );

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category> FindByNameAsync(string name)
        { 
            var category= await _categories.SingleOrDefaultAsync(c => c.Name == name);
             if (category == null)
            {
                throw new KeyNotFoundException($"Category with name {name} not found.");
            }
            return category;
        }
    }
}
