using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Data
{
    public class ProductSeed
    {
        EcommerceDbContext _db;

        public ProductSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Product> SeedProducts(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new Product
                {
                    Id = Guid.Parse(fields[0]),
                    Name = fields[1],
                    Description = fields[2],
                    Price = decimal.Parse(fields[3]),
                    CategoryId = Guid.Parse(fields[4]),
                    Inventory = int.Parse(fields[5]),
                    CreatedAt = DateTime.Parse(fields[6]),
                    UpdatedAt = DateTime.Parse(fields[7])
                };
            });
        }

    }
}