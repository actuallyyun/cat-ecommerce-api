using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepo = new Mock<IOrderRepository>();
        private Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
        private Mock<IAddressRepository> _mockAddressRepo = new Mock<IAddressRepository>();
        private Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly OrderService _orderService;

        public static readonly IEnumerable<object[]> InvalidOrderCreateData =
            TestUtils.InvalidOrderCreateData;

        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _mockOrderRepo.Object,
                _mockUserRepo.Object,
                _mockAddressRepo.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async void CreateOrder_WithValidData_ShouldCreateAndReturnOrderReadDto()
        {
            var orderDto = new OrderCreateDto(
                TestUtils.user.Id,
                TestUtils.address1.Id,
                OrderStatus.Created,
                [TestUtils.orderItemDto1, TestUtils.orderItemDto2]
            );
            _mockUserRepo
                .Setup(x => x.GetUserByIdAsync(TestUtils.user.Id))
                .ReturnsAsync(TestUtils.user);
            _mockAddressRepo
                .Setup(x => x.GetAddressByIdAsync(TestUtils.address1.Id))
                .ReturnsAsync(TestUtils.address1);
            _mockMapper.Setup(x => x.Map<Order>(orderDto)).Returns(TestUtils.order);
            _mockOrderRepo
                .Setup(x => x.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(TestUtils.order);

            var res = await _orderService.CreateOrderAsync(orderDto);
            Assert.Equal(TestUtils.order.Id, res.Id);
            Assert.Equal(res.Items.Count, 2);
            Assert.Equal(TestUtils.user.Id, res.UserId);
        }

        [Theory]
        [MemberData(nameof(InvalidOrderCreateData))]
        public async Task CreateOrder_WithValidData_ShouldThrowError(OrderCreateDto createDto)
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.CreateOrderAsync(createDto)
            );
        }

        [Fact]
        public async Task DeleteOrder_WithValidData_ShouldDeleteAndReturnTrue()
        {
            _mockOrderRepo
                .Setup(x => x.GetOrderByIdAsync(TestUtils.order.Id))
                .ReturnsAsync(TestUtils.order);

            var res = await _orderService.DeleteOrderByIdAsync(TestUtils.order.Id);
            Assert.True(res);
            _mockOrderRepo.Verify(x => x.DeleteOrderByIdAsync(TestUtils.order.Id), Times.Once());
        }

        [Fact]
        public async Task DeleteOrder_WithInValidData_ShouldThrowError()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.DeleteOrderByIdAsync(Guid.NewGuid())
            );
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetAllUserOrders_WithValidAndInvalidData_ShouldReturnOrThrowError(
            bool exists
        )
        {
            _mockUserRepo
                .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exists ? TestUtils.user : null);

            if (!exists)
            {
                Assert.ThrowsAsync<ArgumentException>(
                    () => _orderService.GetAllUserOrdersAsync(Guid.NewGuid(), null)
                );
            }
            else
            {
                _mockOrderRepo
                    .Setup(x => x.GetAllUserOrdersAsync(TestUtils.user.Id, null))
                    .ReturnsAsync([TestUtils.order]);

                var res = await _orderService.GetAllUserOrdersAsync(TestUtils.user.Id, null);

                Assert.Single(res);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetOrderById_WithInvalidAndValidData_ShouldThrowErrorOrReturnOrder(
            bool exists
        )
        {
            _mockOrderRepo
                .Setup(x => x.GetOrderByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exists ? TestUtils.order : null);

            if (!exists)
            {
                await Assert.ThrowsAsync<ArgumentException>(
                    () => _orderService.GetOrderByIdAsync(Guid.NewGuid())
                );
            }
            else
            {
                var res = await _orderService.GetOrderByIdAsync(Guid.NewGuid());
                Assert.Equal(TestUtils.order.Id, res.Id);
                Assert.Equal(TestUtils.order.AddressId, res.AddressId);
            }
        }

        [Fact]
        public async Task UpdateOrder_WithValidData_ShouldUpdateAndReturnTrue()
        {
            var orderUpdateDto = new OrderUpdateDto(TestUtils.address2.Id, OrderStatus.Cancelled);

            _mockOrderRepo
                .Setup(x => x.GetOrderByIdAsync(TestUtils.order.Id))
                .ReturnsAsync(TestUtils.order);

            _mockOrderRepo.Setup(x => x.UpdateOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

            var res = await _orderService.UpdateOrderByIdAsync(TestUtils.order.Id, orderUpdateDto);

            TestUtils.order.AddressId = (Guid)orderUpdateDto.AddressId;
            TestUtils.order.Status = (OrderStatus)orderUpdateDto.Status;

            Assert.True(res);
            _mockOrderRepo.Verify(x => x.UpdateOrderAsync(TestUtils.order), Times.Exactly(1));
        }

        [Fact]
        public async Task UpdateOrder_WithInValidOrderId_ShouldThrowError()
        {
            var orderUpdateDto = new OrderUpdateDto(TestUtils.address2.Id, OrderStatus.Cancelled);

            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _orderService.UpdateOrderByIdAsync(Guid.NewGuid(), orderUpdateDto)
            );
            Assert.Contains("Order not found", ex.Message);
        }

        [Fact]
        public async Task UpdateOrder_WithInValidOrder_ShouldThrowError()
        {
            var orderUpdateDto = new OrderUpdateDto(TestUtils.address2.Id, OrderStatus.Cancelled);
            _mockOrderRepo.Setup(x=>x.GetOrderByIdAsync(TestUtils.InvalidOrderToUpdate.Id)).ReturnsAsync(TestUtils.InvalidOrderToUpdate);
            
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () =>
                    _orderService.UpdateOrderByIdAsync(
                        TestUtils.InvalidOrderToUpdate.Id,
                        orderUpdateDto
                    )
            );
            Assert.Contains("Cannot update order with status Cancelled", ex.Message);
        }
    }
}
