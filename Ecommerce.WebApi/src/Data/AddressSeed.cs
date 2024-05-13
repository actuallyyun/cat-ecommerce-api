using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Data
{
    public class AddressSeed
    {
        EcommerceDbContext _db;

        public AddressSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<Address> SeedAddresses(string filePath)
        {

            return _db.ReadFileSeedData(filePath, fields =>
            {

                return new Address
                {
                    Id = Guid.Parse(fields[0]),
                    UserId = Guid.Parse(fields[1]),
                    AddressLine = fields[2],
                    PostalCode = fields[3],
                    Country = fields[4],
                    PhoneNumber = fields[5]
                };
            });
        }

    }
}