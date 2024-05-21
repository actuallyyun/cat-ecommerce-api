using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IProductImageService
    {
        Task<ProductImage> CreateImageAsync(ImageCreateDto createDto);
        Task<bool> UpdateImageAsync(Guid imageId, ImageUpdateDto updateDto);
        Task<ProductImage> GetByIdAsync(Guid imageId);
        Task<bool> DeleteImageAsync(Guid imageId);
    }
}
