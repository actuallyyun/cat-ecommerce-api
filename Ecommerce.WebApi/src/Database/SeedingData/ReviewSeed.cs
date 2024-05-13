using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.WebApi.src.Data
{
    public class ReviewSeed
    {
        EcommerceDbContext _db;

        public ReviewSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Review> SeedReviews(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new Review
                {
                    Id = Guid.Parse(fields[0]),
                    UserId = Guid.Parse(fields[1]),
                    ProductId = Guid.Parse(fields[2]),
                    IsAnonymous = bool.Parse(fields[3]),
                    Content = fields[4],
                    Rating = int.Parse(fields[5])
                };
            });
        }
    }
}