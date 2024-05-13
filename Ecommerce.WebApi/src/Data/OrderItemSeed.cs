using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Data
{
    public class OrderItemSeed
    {
        EcommerceDbContext _db;

        public OrderItemSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<OrderItem> SeedOrderItem(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new OrderItem
                {
                    Id = Guid.Parse(fields[0]),
                    ProductId = Guid.Parse(fields[1]),
                    OrderId = Guid.Parse(fields[2]),
                    Quantity = int.Parse(fields[3]),
                    Price = decimal.Parse(fields[4])
                };
            });
        }
    }
}