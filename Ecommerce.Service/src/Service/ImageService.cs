
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ImageService : IImageService
    {
        public Task<Image> CreateImageAsync(ImageCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteImageAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetByIdAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Image>> GetImagesByProductIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateImageAsync(Guid imageId, ImageUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}