using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IImageRepository
    {
        Task<Image> CreateAsync(Image image);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
