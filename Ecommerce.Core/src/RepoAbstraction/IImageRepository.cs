using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IProductImageRepository
    {
        Task<ProductImage> CreateAsync(ProductImage image);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
