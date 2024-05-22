using System.Data.Common;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class ProductRepo : IProductRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Product> _products;
        private readonly DbSet<ProductImage> _images;

        private readonly DbSet<Category> _categories;
    
        public ProductRepo(EcommerceDbContext context)
        {
            _context = context;
            _products = _context.Products;
            _images = _context.ProductImages;
            _categories=_context.Categories;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _products.AddAsync(product);
                    foreach(var image in product.Images){
                        await _images.AddAsync(image);
                    }
                    await _context.SaveChangesAsync();
                    return product;
                }
                catch (DbException)
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProductByIdAsync(Guid id)
        {
            await _products.Where(p => p.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(QueryOptions? options)
        {
            var searchKey = options?.SearchKey ?? null;
            var skipFrom = (options?.StartingAfter == null ? options?.StartingAfter : 0) + 1;
            var sortBy = options?.SortBy ?? AppConstants.DEFAULT_SORT_BY;

            IEnumerable<Product> products;

            if (options?.SortOrder == SortOrder.ASC)
            {
                products = await _products
                .Include(p=>p.Category)
                .Include(p=>p.Images)
                    .Where(p => p.Title.Contains(searchKey))
                    .Skip(skipFrom ?? 1)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderBy(p => sortBy)
                    .ToListAsync();
            }
            else
            {
                products = await _products
                    .Where(p => p.Title.Contains(searchKey))
                    .Skip(skipFrom ?? 1)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderByDescending(p => sortBy)
                    .ToListAsync();
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid catId){
            var products= await _products.Where(p=>p.CategoryId==catId).OrderByDescending(p=>p.Price).ToListAsync();
            foreach(var product in products){
                var images=await _images.Where(im=>im.ProductId==product.Id).ToListAsync();
                product.Images=images;
                var category=await _categories.SingleOrDefaultAsync(ca=>ca.Id==product.CategoryId);
            }
            return products;
        }
        public async Task<Product>? GetProductByIdAsync(Guid id)
        {
            var product= await _products.Include(p=>p.Category).SingleOrDefaultAsync(p => p.Id == id);
            var images=await _images.Where(i=>i.ProductId==id).ToListAsync();
         
            product.Images=images;
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var productFound = await _products
                        .Where(p => p.Id == product.Id)
                        .ExecuteUpdateAsync(setters =>
                            setters
                                .SetProperty(p => p.CategoryId, product.CategoryId)
                                .SetProperty(p => p.Description, product.Description)
                                .SetProperty(p => p.Inventory, product.Inventory)
                                .SetProperty(p => p.Title, product.Title)
                                .SetProperty(p => p.Price, product.Price)
                        );
                    foreach (var image in product.Images)
                    {
                        var foundImage = await _images
                            .Where(i => i.Id == image.Id)
                            .ExecuteUpdateAsync(setters =>
                                setters.SetProperty(i => i.Url, image.Url)
                            );
                    }
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
