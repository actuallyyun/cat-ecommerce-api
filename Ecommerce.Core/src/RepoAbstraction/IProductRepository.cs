using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<Product>? GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync(QueryOptions options);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid catId);
        Task<bool> DeleteProductByIdAsync(Guid id);

    }
}