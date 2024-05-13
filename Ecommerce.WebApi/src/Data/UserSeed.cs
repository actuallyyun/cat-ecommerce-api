using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.WebApi.src.Data
{
    using System;
    using System.Collections.Generic;

    public class UserSeed
    {

        EcommerceDbContext _db;

        public UserSeed(EcommerceDbContext db)
        {
            _db = db;
        }
        public List<User> SeedUsers(string filePath)
        {
            return _db.ReadFileSeedData(filePath, fields =>
            {
                return new User
                {
                    Id = Guid.Parse(fields[0]),
                    FirstName = fields[1],
                    LastName = fields[2],
                    Role = Enum.Parse<UserRole>(fields[3], true),
                    Avatar = fields[4],
                    Email = fields[5],
                    Password = fields[6]
                };
            });
        }


    }

}