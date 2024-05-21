
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class ProductImageRepo : IProductImageRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<ProductImage> _images;

        public ProductImageRepo(EcommerceDbContext context){
            _context=context;
            _images=context.ProductImages;
        }

        public async Task<ProductImage> CreateAsync(ProductImage image)
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