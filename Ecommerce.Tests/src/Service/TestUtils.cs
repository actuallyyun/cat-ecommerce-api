using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Tests.src.Service
{
    public static class TestUtils
    {
        public static Category category = new Category("testCategory", "test-url");
        public static User user = new User
        {
            FirstName = "user1",
            LastName = "test",
            Role = UserRole.User,
            Avatar = "profile_url",
            Email = "user@mail.com",
            Password = "user_pass",
            Salt = []
        };
        public static Address address1 = new Address
        {
            UserId = user.Id,
            AddressLine = "address 1",
            PostalCode = "8492nf",
            Country = "finland",
            PhoneNumber = "76830284"
        };

        public static Address address2 = new Address
        {
            UserId = user.Id,
            AddressLine = "address 2",
            PostalCode = "8492nf",
            Country = "finland",
            PhoneNumber = "76830284"
        };

        public static ProductCreateDto InvalidP1 = new ProductCreateDto
        {
            Name = "product",
            Description = "des",
            Price = -3.4m,
            CategoryId = category.Id,
            Inventory = 100,
            ImageCreateDto = []
        };
        public static ProductCreateDto InvalidP2 = new ProductCreateDto
        {
            Name = "product",
            Description = "des",
            Price = 3.4m,
            CategoryId = category.Id,
            Inventory = -100,
            ImageCreateDto = []
        };
        public static ProductCreateDto InvalidP3 = new ProductCreateDto
        {
            Name = "product",
            Description = "des",
            Price = -3.4m,
            CategoryId = category.Id,
            Inventory = 0,
            ImageCreateDto = []
        };

        public static ProductCreateDto InvalidP4 = new ProductCreateDto
        {
            Name = "product",
            Description = "des",
            Price = 3.4m,
            CategoryId = category.Id,
            Inventory = 100,
            ImageCreateDto = []
        };

        public static IEnumerable<object[]> InvalidProductCreateData =>
            [
                new object[] { InvalidP1 },
                new object[] { InvalidP2 },
                new object[] { InvalidP3 },
                new object[] { InvalidP4 }
            ];

        public static Product Product1 = new Product
        {
            Name = "product",
            Description = "des",
            CategoryId = category.Id,
            Price = 3.4m,
            Inventory = 100
        };

        public static Product Product2 = new Product
        {
            Name = "product2",
            Description = "des",
            CategoryId = category.Id,
            Price = 30.4m,
            Inventory = 100
        };
        public static ProductUpdateDto InvalidUpdateP1 = new ProductUpdateDto { Price = -3.3m, };

        public static ProductUpdateDto InvalidUpdateP2 = new ProductUpdateDto{Inventory=-10};
        public static ProductUpdateDto InvalidUpdateP3 = new ProductUpdateDto{
            CategoryId=Guid.NewGuid()
        };

        public static IEnumerable<object[]> InvalidProductUpdateData =>
            [
                new object[] { InvalidUpdateP1 },
                new object[] { InvalidUpdateP2 },
                new object[] { InvalidUpdateP3 },
            ];

        public static ProductUpdateDto ProductUpdate = new ProductUpdateDto{
            Name="updated product",
            Description="update me",
            Price=5.6m,
            CategoryId=category.Id,
            Inventory=300,
        
        };

        public static Order order = new Order{UserId=user.Id, AddressId=address1.Id, Status=OrderStatus.Created};
        public static OrderItemCreateDto orderItemDto1 = new OrderItemCreateDto{
            ProductId=Product1.Id, 
            Quantity=4, 
            Price=4.5m};
        public static OrderItemCreateDto orderItemDto2 = new OrderItemCreateDto{
            ProductId=Product2.Id, 
            Quantity=4, 
            Price=4.5m};
        public static OrderItem orderItem = new OrderItem{
            
            ProductId=Product1.Id, 
            OrderId=order.Id, 
            Quantity=3, 
            Price=4.5m};

        public static OrderCreateDto invalidOrderDto1 = new OrderCreateDto{
        UserId=Guid.NewGuid(),
            AddressId=address1.Id,
        };
        public static OrderCreateDto invalidOrderDto2 = new OrderCreateDto{
            UserId=user.Id,
            AddressId=Guid.NewGuid(),
           
        };

        public static Order InvalidOrderToUpdate = new Order{
           UserId= user.Id,
            AddressId=address1.Id,
            Status=OrderStatus.Completed
        };
        public static IEnumerable<object[]> InvalidOrderCreateData =>
            [new object[] { invalidOrderDto1 }, new object[] { invalidOrderDto2 }];
    }

    public static class OrderValidator
    {
        public static void ValidateOrder(Order order)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), order.Status))
            {
                throw new ArgumentException("Invalid Order Status");
            }
        }

        public static void ValidateOrderItem(OrderItem item)
        {
            if (item.Quantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(item.Quantity),
                    "Quantity must be greater than 0"
                );
            }

            if (item.Price <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(item.Price),
                    "Price must be greater than 0.00"
                );
            }
        }
    }
}
