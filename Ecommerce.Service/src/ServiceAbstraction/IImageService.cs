using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IImageService
    {
        Task<Image> CreateImageAsync(ImageCreateDto createDto);
        Task<bool> UpdateImageAsync(Guid imageId,ImageUpdateDto updateDto);
        Task<IEnumerable<Image>> GetImagesByProductIdAsync(Guid productId);
        Task<Image>GetByIdAsync(Guid imageId);
        Task<bool> DeleteImageAsync(Guid imageId);
    }
}