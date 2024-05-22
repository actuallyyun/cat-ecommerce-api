
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ImageService : IProductImageService
    {
        public Task<ProductImage> CreateImageAsync(ImageCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteImageAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductImage> GetByIdAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

    }
}