using System.Text;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.WebApi.src.Database.SeedingData
{
    public class SeedingData
    {
        // first read file from csv
        // then parse it to different objects
        public static List<Category> GetCategories()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/category.csv",
                fields =>
                {
                    return new Category { Id = Guid.Parse(fields[0]), Name = fields[1], };
                }
            );
        }

        public static List<User> GetUsers()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/users.csv",
                fields =>
                {
                    return new User
                    {
                        Id = Guid.Parse(fields[0]),
                        FirstName = fields[1],
                        LastName = fields[2],
                        Role = Enum.Parse<UserRole>(fields[3], true),
                        Avatar = fields[4],
                        Email = fields[5],
                        Password = fields[6],
                        Salt=Encoding.UTF8.GetBytes(fields[7])
                    };
                }
            );
        }

        public static List<Product> GetProducts()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/products.csv",
                fields =>
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
                }
            );
        }

        //public static List<Image> GetImages()
        //{
        //    return ReadCsvData(
        //        "./src/Database/SeedingData/csv/images.csv",
        //        fields =>
        //        {
        //            return new Image
        //            {
        //                Id = Guid.Parse(fields[0]),
        //                Url = fields[2]
        //            };
        //        }
        //    );
        //}

        public static List<Address> GetAddresses()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/addresses.csv",
                fields =>
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
                }
            );
        }

        public static List<Order> GetOrders()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/orders.csv",
                fields =>
                {
                    return new Order
                    {
                        Id = Guid.Parse(fields[0]),
                        UserId = Guid.Parse(fields[1]),
                        AddressId = Guid.Parse(fields[2]),
                        Status = Enum.Parse<OrderStatus>(fields[3])
                    };
                }
            );
        }

        public static List<OrderItem> GetOrderItems()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/order_items.csv",
                fields =>
                {
                    return new OrderItem
                    {
                        Id = Guid.Parse(fields[0]),
                        ProductId = Guid.Parse(fields[1]),
                        OrderId = Guid.Parse(fields[2]),
                        Quantity = int.Parse(fields[3]),
                        Price = decimal.Parse(fields[4])
                    };
                }
            );
        }

        public static List<Review> GetReviews()
        {
            return ReadCsvData(
                "./src/Database/SeedingData/csv/reviews.csv",
                fields =>
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
                }
            );
        }

        private static List<T> ReadCsvData<T>(string path, Func<string[], T> createObject)
        {
            var dataList = new List<T>();

            try
            {
                using (var reader = new StreamReader(path))
                {
                    // Read the first line (header) and discard it
                    reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var fields = line.Split(';');
                        var dataObject = createObject(fields);
                        dataList.Add(dataObject);
                    }
                }

                return dataList;
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:", path);
                throw new IOException(path, e);
            }
        }
    }
}
