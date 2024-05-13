using Moq;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.Service;

namespace Ecommerce.Tests.src.Service
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepo.Object);
        }

        [Fact]
        public async Task CreateCategoryAsync_WithUniqueName_ShouldCreateAndReturnCategory()
        {
            var categoryDto = new CategoryCreateDto { Name = "New Category", Image = "http://example.com/image.jpg" };
            var expectedCategory = new Category(categoryDto.Name, categoryDto.Image);

            _mockCategoryRepo.Setup(r => r.FindByNameAsync(categoryDto.Name)).ReturnsAsync((Category)null);
            _mockCategoryRepo.Setup(r => r.CreateCategoryAsync(It.IsAny<Category>())).ReturnsAsync(expectedCategory);

            var result = await _categoryService.CreateCategoryAsync(categoryDto);

            Assert.NotNull(result);
            Assert.Equal(categoryDto.Name, result.Name);
            Assert.Equal(categoryDto.Image, result.Image);
            _mockCategoryRepo.Verify(r => r.FindByNameAsync(categoryDto.Name), Times.Once);
            _mockCategoryRepo.Verify(r => r.CreateCategoryAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_WithExistingName_ShouldThrowArgumentException()
        {
            var categoryDto = new CategoryCreateDto { Name = "Existing Category", Image = "http://example.com/image.jpg" };
            var existingCategory = new Category(categoryDto.Name, categoryDto.Image);

            _mockCategoryRepo.Setup(r => r.FindByNameAsync(categoryDto.Name)).ReturnsAsync(existingCategory);

            await Assert.ThrowsAsync<ArgumentException>(() => _categoryService.CreateCategoryAsync(categoryDto));
            _mockCategoryRepo.Verify(r => r.FindByNameAsync(categoryDto.Name), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WithValidId_ShouldUpdateAndReturnTrue()
        {
            var categoryId = Guid.NewGuid();
            var categoryUpdateDto = new CategoryUpdateDto { Name = "Updated Category", Image = "http://example.com/updated.jpg" };
            var existingCategory = new Category("Old Category", "http://example.com/old.jpg");

            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mockCategoryRepo.Setup(r => r.UpdateCategoryAsync(categoryId, existingCategory)).ReturnsAsync(true);

            var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryUpdateDto);

            Assert.True(result);
            Assert.Equal(categoryUpdateDto.Name, existingCategory.Name);
            Assert.Equal(categoryUpdateDto.Image, existingCategory.Image);
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
            _mockCategoryRepo.Verify(r => r.UpdateCategoryAsync(categoryId, existingCategory), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_WithNonExistentId_ShouldThrowArgumentException()
        {
            var nonExistentId = Guid.NewGuid();
            var categoryUpdateDto = new CategoryUpdateDto { Name = "Nonexistent Update", Image = "http://example.com/doesnotexist.jpg" };

            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(nonExistentId)).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _categoryService.UpdateCategoryAsync(nonExistentId, categoryUpdateDto));
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(nonExistentId), Times.Once);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithValidId_ShouldReturnCategoryReadDto()
        {
            var categoryId = Guid.NewGuid();
            var existingCategory = new Category("Sample Category", "http://example.com/sample.jpg");

            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(existingCategory);

            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            Assert.NotNull(result);
            Assert.Equal(existingCategory.Id, result.Id);
            Assert.Equal(existingCategory.Name, result.Name);
            Assert.Equal(existingCategory.Image, result.Image);
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WithNonExistentId_ShouldThrowKeyNotFoundException()
        {
            var nonExistentId = Guid.NewGuid();
            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(nonExistentId)).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.GetCategoryByIdAsync(nonExistentId));
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(nonExistentId), Times.Once);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategoryReadDtos()
        {
            var categories = new List<Category>
            {
                new Category("Category 1", "http://example.com/image1.jpg"),
                new Category("Category 2", "http://example.com/image2.jpg")
            };

            _mockCategoryRepo.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllCategoriesAsync();

            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            _mockCategoryRepo.Verify(r => r.GetAllCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_WithValidId_ShouldReturnTrue()
        {
            var categoryId = Guid.NewGuid();
            var existingCategory = new Category("Category to Delete", "http://example.com/delete.jpg");

            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(categoryId)).ReturnsAsync(existingCategory);
            _mockCategoryRepo.Setup(r => r.DeleteCategoryAsync(categoryId)).ReturnsAsync(true);

            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            Assert.True(result);
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(categoryId), Times.Once);
            _mockCategoryRepo.Verify(r => r.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_WithNonExistentId_ShouldThrowKeyNotFoundException()
        {
            var nonExistentId = Guid.NewGuid();
            _mockCategoryRepo.Setup(r => r.GetCategoryByIdAsync(nonExistentId)).ReturnsAsync((Category)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _categoryService.DeleteCategoryAsync(nonExistentId));
            _mockCategoryRepo.Verify(r => r.GetCategoryByIdAsync(nonExistentId), Times.Once);
        }
    }
}
