using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepo,
            IMapper mapper
        )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepo;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreate)
        {
            await ValidateIdAsync(productCreate.CategoryId,"Category");
           
            var newProduct = _mapper.Map<Product>(productCreate);

            var product= await _productRepository.CreateProductAsync(newProduct);
            return _mapper.Map<ProductReadDto>(product);
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

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions options)
        {
            var products= await _productRepository.GetAllProductsAsync(options);

            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetProductByIdAsync(Guid id)
        {
            var productFound =await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductReadDto>(productFound);
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
            }
            if (product.Description != null)
            {
                productFound.Description = product.Description;
            }
            if (product.Title != null)
            {
                productFound.Title = product.Title;
            }
            //if (product.Images != null)
            //{
            //    productFound.Images.Clear();
            //    productFound.Images.AddRange(
            //        product.Images.Select(data => new ProductImage(productFound.Id, data))
            //    );
            //}
            return await _productRepository.UpdateProductAsync(productFound);
        }

        public async Task<IEnumerable<ReviewReadDto>> GetAllReviews(Guid id)
        {
            var reviews= await _reviewRepository.GetReviewsByProductIdAsync(id);
            
            return _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }

        private async Task ValidateIdAsync(Guid id, string entityType)
        {
            bool exists = entityType switch
            {
                "Category" => await _categoryRepository.GetCategoryByIdAsync(id) != null,
                "Product" => await _productRepository.GetProductByIdAsync(id) != null,
                _ => throw new ArgumentException("Unknown entity type")
            };

            if (!exists)
            {
                throw new ArgumentException($"{entityType} with ID {id} does not exist.");
            }
        }
    }
}
