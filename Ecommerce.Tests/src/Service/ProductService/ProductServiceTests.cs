using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepo = new Mock<IProductRepository>();
        private Mock<ICategoryRepository> _mockCategoryRepo = new Mock<ICategoryRepository>();
        private readonly ProductService _productService;
        public static IEnumerable<object[]> InValidProductCreateData =>
            TestUtils.InvalidProductCreateData;

        public static IEnumerable<object[]> InValidProductUpdateData =>
            TestUtils.InvalidProductUpdateData;

        public ProductServiceTests()
        {
            _productService = new ProductService(_mockProductRepo.Object, _mockCategoryRepo.Object);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ShouldCreateAndReturnProductReadDto()
        {
            var category = new Category("food", "url");
            var testProduct = new Product("product", "des", category, 1.2m, 20);
            _mockProductRepo
                .Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(testProduct);
            _mockCategoryRepo
                .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(category);
            var validProduct = new ProductCreateDto(
                "product",
                "description",
                2.3m,
                category.Id,
                20,
                ["http//img.jpg"]
            );
            var res = await _productService.CreateProductAsync(validProduct);
            Assert.NotNull(res);
            Assert.IsType<ProductReadDto>(res);
            Assert.Equal(validProduct.Name, res.Name);
        }

        [Theory]
        [MemberData(nameof(InValidProductCreateData))]
        public async Task CreateProduct_WithInValidData_ShouldThrowException(ProductCreateDto data)
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(
                () => _productService.CreateProductAsync(data)
            );
        }

        [Fact]
        public async Task DeleteProduct_WithValidData_ShouldDeleteAndReturnTrue()
        {
            _mockProductRepo
                .SetupSequence(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(TestUtils.Product1);

            _mockProductRepo
                .SetupSequence(x => x.DeleteProductByIdAsync(TestUtils.Product1.Id))
                .ReturnsAsync(true);

            var res = await _productService.DeleteProductByIdAsync(TestUtils.Product1.Id);
            Assert.True(res);
            _mockProductRepo.Verify(
                x => x.DeleteProductByIdAsync(It.IsAny<Guid>()),
                Times.AtLeastOnce()
            );
        }

        [Fact]
        public async Task DeleteProduct_WithInValidData_ShouldThrowException()
        {
            _mockProductRepo
                .Setup(x => x.DeleteProductByIdAsync(It.IsAny<Guid>()))
                .Throws<ArgumentException>();
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _productService.DeleteProductByIdAsync(Guid.NewGuid())
            );
            Assert.Contains("Product not found", ex.Message);
        }

        [Theory]
        [MemberData(nameof(InValidProductUpdateData))]
        public async Task UpdateProduct_WithInValidUpdateData_ShouldThrowException(
            ProductUpdateDto data
        )
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _productService.UpdateProductByIdAsync(TestUtils.Product1.Id, data)
            );
        }

        [Fact]
        public async Task UpdateProduct_WithInValidProductId_ShouldThrowException()
        {
            Product? product = null;
            _mockProductRepo
                .SetupSequence(x => x.GetProductByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(product);
            await Assert.ThrowsAsync<ArgumentException>(
                () =>
                    _productService.UpdateProductByIdAsync(Guid.NewGuid(), TestUtils.ProductUpdate)
            );
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ShouldUpdateAndReturnTrue()
        {
            _mockProductRepo
                .SetupSequence(x => x.GetProductByIdAsync(TestUtils.Product1.Id))
                .ReturnsAsync(TestUtils.Product1);
            _mockProductRepo
                .SetupSequence(x => x.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);
            _mockCategoryRepo
                .SetupSequence(x => x.GetCategoryByIdAsync(TestUtils.category.Id))
                .ReturnsAsync(TestUtils.category);
            var res = await _productService.UpdateProductByIdAsync(
                TestUtils.Product1.Id,
                TestUtils.ProductUpdate
            );
            Assert.True(res);
            _mockProductRepo.Verify(
                x => x.UpdateProductAsync(It.IsAny<Product>()),
                Times.Once()
            );
        }
    }
}
