using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepo
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepo;
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto product)
        {
            var categoryFound = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            if (categoryFound == null)
            {
                throw new ArgumentException($"Category with id {product.CategoryId} not found.");
            }
            var productCreate = new Product(
                product.Name,
                product.Description,
                categoryFound,
                product.Price,
                product.Inventory
            );

            foreach (var image in product.Images)
            {
                productCreate.Images.Add(new ProductImage(productCreate.Id, image));
            }
            var newProduct = await _productRepository.CreateProductAsync(productCreate);

            if (newProduct == null)
            {
                throw new ArgumentException("create new product failed.");
            }

            return newProduct;
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            var producFound = await _productRepository.GetProductByIdAsync(id);
            if (producFound == null)
            {
                throw new ArgumentException("Product not found");
            }
            return await _productRepository.DeleteProductByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(QueryOptions options)
        {
            return await _productRepository.GetAllProductsAsync(options);
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<bool> UpdateProductByIdAsync(Guid id, ProductUpdateDto product)
        {
            var productFound = await _productRepository.GetProductByIdAsync(id);
            if (productFound == null)
            {
                throw new ArgumentException("product not found");
            }
            if (product.Price != null)
            {
                productFound.Price = (decimal)product.Price;
            }
            if (product.Inventory != null)
            {
                productFound.Inventory = (int)product.Inventory;
            }
            if (product.CategoryId != null)
            {
                var categoryFound = await _categoryRepository.GetCategoryByIdAsync(
                    (Guid)product.CategoryId
                );
                if (categoryFound == null)
                {
                    throw new ArgumentException("category not found");
                }
                productFound.Category = categoryFound;
            }
            if (product.Description != null)
            {
                productFound.Description = product.Description;
            }
            if (product.Name != null)
            {
                productFound.Name = product.Name;
            }
            if (product.Images != null)
            {
                productFound.Images.Clear();
                productFound.Images.AddRange(
                    product.Images.Select(data => new ProductImage(productFound.Id, data))
                );
            }
            return await _productRepository.UpdateProductAsync(productFound);
        }

        public async Task<IEnumerable<Review>> GetAllReviews(Guid id)
        {
            return await _reviewRepository.GetReviewsByProductIdAsync(id);
        }
    }
}
