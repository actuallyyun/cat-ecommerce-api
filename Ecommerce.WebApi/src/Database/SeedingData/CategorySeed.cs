using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Data
{
    public class CategorySeed
    {
        EcommerceDbContext _db;

        public CategorySeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Category> SeedCategories(string filePath)
        {
            return _db.ReadFileSeedData(filePath, fields =>
            {
                return new Category
                {
                    Id = Guid.Parse(fields[0]),
                    Name = fields[1],
                };
            });
        }
    }
}