using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Data
{
    public class ImageSeed
    {
        EcommerceDbContext _db;

        public ImageSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Image> SeedImage(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new Image
                {
                    Id = Guid.Parse(fields[0]),
                    EntityId = Guid.Parse(fields[1]),
                    Url = fields[2]
                };
            });
        }
    }
}