using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto product)
        {
            var categoryFound = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            if (categoryFound == null)
            {
                throw new ArgumentException($"Category with id {product.CategoryId} not found.");
            }
            if (product.Price < 0)
            {
                throw new ArgumentException("Price must be greater than 0.");
            }
            if (product.Inventory < 0)
            {
                throw new ArgumentException("Inventory must be greater than 0");
            }
            var productCreate = new Product(
                product.Name,
                product.Description,
                categoryFound,
                product.Price,
                product.Inventory
            );

            foreach (string image in product.Images)
            {
                productCreate.Images.Add(new Image(productCreate.Id, image));
            }
            var newProduct = await _productRepository.CreateProductAsync(productCreate);

            if (newProduct == null)
            {
                throw new ArgumentException("create new product failed.");
            }

            return new ProductReadDto(
                newProduct.Id,
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                categoryFound,
                newProduct.Inventory,
                newProduct.Images
            );
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

        public Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<ProductReadDto> GetProductByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductByIdAsync(Guid id, ProductUpdateDto product)
        {
            var productFound = await _productRepository.GetProductByIdAsync(id);
            if (productFound == null)
            {
                throw new ArgumentException("product not found");
            }
            // TODO could refactor this to a helper function
            if (product.Price != null)
            {
                if (product.Price < 0)
                {
                    throw new ArgumentException("Price must be greater than 0.");
                }
                productFound.Price = (decimal)product.Price;
            }
            if (product.Inventory != null)
            {
                if (product.Inventory < 0)
                {
                    throw new ArgumentException("Inventory must be greater than 0");
                }

                productFound.Inventory = (int)product.Inventory;
            }
            if (product.CategoryId != null)
            {
                var categoryFound = await _categoryRepository.GetCategoryByIdAsync((Guid)product.CategoryId);
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
                productFound.Images.AddRange(product.Images.Select(url => new Image(productFound.Id, url)));
            }
            return await _productRepository.UpdateProductAsync(productFound);
        }
    }
}