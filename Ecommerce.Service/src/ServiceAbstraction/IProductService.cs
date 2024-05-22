using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IProductService
    {
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto product);
        Task<bool> UpdateProductByIdAsync(Guid id, ProductUpdateDto product);
        Task<ProductReadDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions? options);
        Task<bool> DeleteProductByIdAsync(Guid id);
        Task<IEnumerable<ReviewReadDto>> GetAllReviews(Guid id);
    }
}
