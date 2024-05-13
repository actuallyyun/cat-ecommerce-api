using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Tests.src.Service;
namespace Ecommerce.Tests.src.Core
{
    public class OrderTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var userId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var status = OrderStatus.Created;

            var order = new Order(userId, addressId, status);

            Assert.Equal(userId, order.UserId);
            Assert.Equal(addressId, order.AddressId);
            Assert.Equal(status, order.Status);
            Assert.NotNull(order.Items);
        }

        [Fact]
        public void Constructor_WithInvalidStatus_ShouldThrowArgumentException()
        {
            var userId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var invalidStatus = (OrderStatus)(-1); // Invalid status value

            var order = new Order(userId, addressId, invalidStatus);

            Assert.Throws<ArgumentException>(() => OrderValidator.ValidateOrder(order));
        }

        [Fact]
        public void AddOrderItem_WithNegativeQuantity_ShouldThrowRangeException()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Created);

            var item = new OrderItem(Guid.NewGuid(), order.Id, -1, 10.99m);

            Assert.Throws<ArgumentOutOfRangeException>(() => OrderValidator.ValidateOrderItem(item));
        }

        [Fact]
        public void AddOrderItem_WithZeroPrice_ShouldThrowRangeException()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.Created);

            var item = new OrderItem(Guid.NewGuid(), order.Id, 1, 0.00m);

            Assert.Throws<ArgumentOutOfRangeException>(() => OrderValidator.ValidateOrderItem(item));
        }
    }
}
