
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class ImageRepo : IImageRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Image> _images;

        public ImageRepo(EcommerceDbContext context){
            _context=context;
            _images=context.Images;
        }

        public async Task<Image> CreateAsync(Image image)
        {
            await _images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}