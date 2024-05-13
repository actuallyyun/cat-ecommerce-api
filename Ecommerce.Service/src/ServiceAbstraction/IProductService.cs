using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductCreateDto product);
        Task<bool> UpdateProductByIdAsync(Guid id,ProductUpdateDto product);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync(QueryOptions? options);
        Task<bool> DeleteProductByIdAsync(Guid id);
    }
}
