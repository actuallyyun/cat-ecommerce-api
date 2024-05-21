using Bogus;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Service;

namespace Ecommerce.WebApi.src.Database
{
    public class SeedData
    {
        public static List<Category> GenerateCategories()
        {
            var faker = new Faker("en");
            var categories = new List<Category>();
            List<string> categoryList =
            [
                "Electronics",
                "Fashion",
                "Home & Kitchen",
                "Beauty & Personal Care",
                "Books",
                "Toys & Games",
                "Sports & Outdoors",
                "Health & Wellness",
                "Automotive",
                "Groceries",
                "Pet Supplies",
                "Office Supplies",
                "Jewelry",
                "Music & Movies",
                "Tools & Home Improvement",
                "Baby Products",
                "Garden & Outdoor",
                "Luggage & Travel Gear",
                "Crafts & Sewing",
                "Computers & Accessories"
            ];

            foreach (var cate in categoryList)
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = cate,
                    Image = faker.Image.PicsumUrl(),
                };
                categories.Add(category);
            }

            return categories;
        }

        public static List<User> GenerateUsers()
        {
            var passwordService = new PasswordService();
            var users = new List<User>();

            // customers
            for (int i = 0; i < 100; i++)
            {
                var faker = new Faker();
                var userId = Guid.NewGuid();
                var userFirstName = faker.Person.FirstName;
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = userFirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    Password = passwordService.HashPassword(userFirstName, out byte[] salt), // user first name is used as password
                    Salt = salt,
                    Avatar = faker.Person.Avatar,
                    Role = UserRole.User
                };
                users.Add(user);
            }

            // create an admin
            var admin = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@mail.com",
                Password = passwordService.HashPassword("admin", out byte[] adminSalt),
                Salt = adminSalt,
                Avatar = new Faker().Person.Avatar,
                Role = UserRole.Admin,
            };

            users.Add(admin);

            return users;
        }

        public static List<Address> GenerateAddresses(List<User> users)
        {
            var addresses = new List<Address>();

            // users can have 1 to 5 addresses, assigned randomly
            foreach (var user in users)
            {
                int addressCount = new Random().Next(1, 5);
                for (int i = 0; i < addressCount; i++)
                {
                    var faker = new Faker("fi");
                    var address = new Address
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        FirstName = faker.Person.FirstName,
                        LastName = faker.Person.LastName,
                        AddressLine = faker.Address.FullAddress(),
                        Country = faker.Address.Country(),
                        PostalCode = faker.Address.ZipCode(),
                        PhoneNumber = faker.Person.Phone
                    };
                    addresses.Add(address);
                }
            }
            return addresses;
        }

        public static List<Product> GenerateProducts(List<Category> categories)
        {
            var products = new List<Product>();

            foreach (var category in categories)
            {
                for (int i = 0; i < 20; i++)
                {
                    var faker = new Faker();
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Title =
                            $"{faker.Commerce.ProductAdjective()} {faker.Commerce.Product()} {faker.Random.Word()}",
                        Description = faker.Commerce.ProductDescription(),
                        Price = decimal.Parse(faker.Commerce.Price()),
                        Inventory = faker.Commerce.Random.Int(0, 200),
                        CategoryId = category.Id,
                        Rating = new Random().Next(1, 5),
                    };

                    products.Add(product);
                }
            }
            return products;
        }

        public static List<ProductImage> GenerateProductImages(List<Product> products)
        {
            var images = new List<ProductImage>();
            foreach (var product in products)
            {
                for (int i = 0; i < 4; i++)
                {
                    var faker = new Faker("en");
                    var image = new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        Url = faker.Image.PicsumUrl(),
                        ProductId = product.Id,
                    };
                    images.Add(image);
                }
            }

            return images;
        }

        public static List<Order> GenerateOrders(List<User> users, List<Address> addresses)
        {
            var orders = new List<Order>();

            foreach (var user in users)
            {
                int orderCount = new Random().Next(0, 10); // users can have 0-10 orders
                for (int i = 0; i < orderCount; i++)
                {
                    var faker = new Faker("fi");
                    var userAddresses = addresses
                        .Where(address => address.UserId == user.Id)
                        .ToArray();

                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        AddressId = userAddresses[0].Id,
                        Status = faker.PickRandom<OrderStatus>(),
                    };
                    orders.Add(order);
                }
            }

            return orders;
        }

        public static List<OrderItem> GenerateOrderItems(List<Order> orders, List<Product> products)
        {
            var orderItems = new List<OrderItem>();

            foreach (var order in orders)
            {
                var faker = new Faker("en");
                var randomProducts = faker.Random.ListItems(products, 20);
                int productCount = faker.Random.Int(1, 20);
                for (int i = 0; i < productCount; i++)
                {
                    var randomProduct = randomProducts[i];

                    var price = faker.Random.Decimal(1, 1000);
                    var quantity = faker.Random.Int(1, 30);

                    var orderItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = randomProduct.Id,
                        Quantity = quantity,
                        Price = price,
                    };

                    orderItems.Add(orderItem);
                }
            }

            return orderItems;
        }

        public static List<Review> GenerateReviews(List<User> users, List<Product> products)
        {
            var reviews = new List<Review>();

            foreach (var product in products)
            {
                var faker = new Faker("en");
                int reveiwCount = faker.Random.Int(0, 100);
                for (int i = 0; i < reveiwCount; i++)
                {
                    var randomUser = faker.Random.ListItem(users);
                    var review = new Review
                    {
                        Id = Guid.NewGuid(),
                        Rating = faker.Random.Int(1, 5),
                        Content = faker.Lorem.Paragraph(),
                        UserId = randomUser.Id,
                        ProductId = product.Id,
                    };

                    reviews.Add(review);
                }
            }
            return reviews;
        }

        public static List<ReviewImage> GenerateReviewImages(List<Review> reviews)
        {
            var images = new List<ReviewImage>();
            foreach (var review in reviews)
            {
                for (int i = 0; i < 4; i++)
                {
                    var faker = new Faker("en");
                    var image = new ReviewImage
                    {
                        Id = Guid.NewGuid(),
                        Url = faker.Image.PicsumUrl(),
                        ReviewId = review.Id,
                    };
                    images.Add(image);
                }
            }

            return images;
        }
    }
}
