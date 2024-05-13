using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.WebApi.src.Data
{
    public class OrderSeed
    {
        EcommerceDbContext _db;

        public OrderSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Order> SeedOrders(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new Order
                {
                    Id = Guid.Parse(fields[0]),
                    UserId = Guid.Parse(fields[1]),
                    AddressId = Guid.Parse(fields[2]),
                    Status = Enum.Parse<OrderStatus>(fields[3])
                };
            });
        }

    }
}