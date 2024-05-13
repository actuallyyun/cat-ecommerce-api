using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Tests.src.Service
{
    public static class TestUtils
    {
        public static Category category = new Category("testCategory", "test-url");
        public static User user = new User(
            "user1",
            "test",
            UserRole.User,
            "profile_url",
            "user@mail.com",
            "user_pass"
        );
        public static Address address1 = new Address(
            user.Id,
            "address 1",
            "8492nf",
            "finland",
            "76830284"
        );

        public static Address address2 = new Address(
            user.Id,
            "address 2",
            "8492nf",
            "finland",
            "76830284"
        );

        public static ProductCreateDto InvalidP1 = new ProductCreateDto(
            "product",
            "des",
            -3.4m,
            category.Id,
            100,
            ["url"]
        );
        public static ProductCreateDto InvalidP2 = new ProductCreateDto(
            "product",
            "des",
            3.4m,
            category.Id,
            -100,
            ["url"]
        );
        public static ProductCreateDto InvalidP3 = new ProductCreateDto(
            "product",
            "des",
            3.4m,
            category.Id,
            0,
            ["url"]
        );

        public static ProductCreateDto InvalidP4 = new ProductCreateDto(
            "product",
            "des",
            3.4m,
            new Guid(),
            100,
            ["url"]
        );

        public static IEnumerable<object[]> InvalidProductCreateData =>
            [
                new object[] { InvalidP1 },
                new object[] { InvalidP2 },
                new object[] { InvalidP3 },
                new object[] { InvalidP4 }
            ];

        public static Product Product1 = new Product("product", "des", category, 3.4m, 100);

        public static Product Product2 = new Product("product 2", "des", category, 30.8m, 10);


        public static ProductUpdateDto InvalidUpdateP1 = new ProductUpdateDto(
            null,
            null,
            -3.3m,
            null,
            null,
            null
        );

        public static ProductUpdateDto InvalidUpdateP2 = new ProductUpdateDto(
            null,
            null,
            null,
            null,
            -30,
            null
        );
        public static ProductUpdateDto InvalidUpdateP3 = new ProductUpdateDto(
            null,
            null,
            null,
            Guid.NewGuid(),
            null,
            null
        );

        public static IEnumerable<object[]> InvalidProductUpdateData =>
            [
                new object[] { InvalidUpdateP1 },
                new object[] { InvalidUpdateP2 },
                new object[] { InvalidUpdateP3 },
            ];

        public static ProductUpdateDto ProductUpdate = new ProductUpdateDto(
            "updated product",
            "update me",
            5.6m,
            category.Id,
            300,
            null
        );

        public static Order order = new Order(user.Id, address1.Id, OrderStatus.Created);
        public static OrderItemDto orderItemDto1 = new OrderItemDto(Product1.Id, 4, 4.5m);
        public static OrderItemDto orderItemDto2 = new OrderItemDto(Product2.Id, 4, 4.5m);
        public static OrderItem orderItem = new OrderItem(Product1.Id, order.Id, 3, 4.5m);

        public static OrderCreateDto invalidOrderDto1 = new OrderCreateDto(
            Guid.NewGuid(),
            address1.Id,
            OrderStatus.Created,
            [orderItemDto1, orderItemDto2]
        );
        public static OrderCreateDto invalidOrderDto2 = new OrderCreateDto(
            user.Id,
            Guid.NewGuid(),
            OrderStatus.Created,
            [orderItemDto1, orderItemDto2]
        );

        public static Order InvalidOrderToUpdate = new Order(
            user.Id,
            address1.Id,
            OrderStatus.Completed
        );
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
            throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be greater than 0");
        }

        if (item.Price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(item.Price), "Price must be greater than 0.00");
        }
    }
}

}
