using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepo;

        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,IProductRepository productRepo,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper=mapper;
            _productRepo=productRepo;
        }

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {

            var existingCategory = await _categoryRepository.FindByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                throw new ArgumentException($"A category with the name {categoryDto.Name} already exists.");
            }

            var category = new Category(categoryDto.Name, categoryDto.Image);

            await _categoryRepository.CreateCategoryAsync(category);
            return category;
        }


        public async Task<bool> UpdateCategoryAsync(Guid id, CategoryUpdateDto categoryDto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                throw new ArgumentException($"No category found with ID {id}.");

            if (!string.IsNullOrWhiteSpace(categoryDto.Name) && categoryDto.Name != category.Name)
            {
                var existingCategory = await _categoryRepository.FindByNameAsync(categoryDto.Name);
                if (existingCategory != null)
                {
                    throw new ArgumentException($"A category with the name {categoryDto.Name} already exists.");
                }
                category.Name = categoryDto.Name;
            }

            if (categoryDto.Image != null)
            {
                category.Image = categoryDto.Image;
            }

            return await _categoryRepository.UpdateCategoryAsync(id, category);
        }

        public async Task<CategoryReadDto> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            return new CategoryReadDto(
                category.Id,
                category.Name,
                 category.Image
                );
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(category => new CategoryReadDto(
                 category.Id,
                 category.Name,
                 category.Image
        ));
        }

        public async Task<IEnumerable<ProductReadDto>>GetProductsByCategoryAsync(Guid id){
            var products=await _productRepo.GetProductsByCategoryAsync(id);
            
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            return await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
    }
}