using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
private readonly IProductRepository _productRepo;
        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepo,
            IAddressRepository addressRepo,
            IProductRepository productRepo
        )
        {
            _orderRepository = orderRepository;
            _userRepository = userRepo;
            _addressRepository = addressRepo;
            _productRepo=productRepo;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto)
        {
            await ValidateIdAsync(orderDto.UserId,"User");
            await ValidateIdAsync(orderDto.AddressId,"Address");

            var order = new Order(orderDto.UserId,orderDto.AddressId,OrderStatus.Created);

            var items=new List<OrderItem>(); 

            foreach (var item in orderDto.Items)
            {
                
                await ValidateIdAsync(item.ProductId,"Product");

                var orderItem = new OrderItem(item.ProductId, order.Id, item.Quantity, item.Price);
                items.Add(orderItem);
            }
            //order.SetOrderItems(items);

            return await _orderRepository.CreateOrderAsync(order,items);
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid id)
        {
            var orderFound = await _orderRepository.GetOrderByIdAsync(id);
            if (orderFound == null)
            {
                throw new ArgumentException("Cannot delete. Order does not exist");
            }
            await _orderRepository.DeleteOrderByIdAsync(id);
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions? options)
        {
            return await _orderRepository.GetAllOrdersAsync(options);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByUserAsync(Guid userId)
        {
            return await _orderRepository.GetAllUserOrdersAsync(userId);
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }
            return order;
        }

        public async Task<bool> UpdateOrderByIdAsync(Guid id, OrderUpdateDto order)
        {
            var orderFound = await _orderRepository.GetOrderByIdAsync(id);
            if (orderFound == null)
            {
                throw new ArgumentException("Order not found");
            }
            if (order.Status != null)
            {
                if (orderFound.Status != OrderStatus.Created)
                {
                    throw new ArgumentException($"Cannot update order with status {order.Status}");
                }

                orderFound.Status = (OrderStatus)order.Status;

                if (order.AddressId != null && order.AddressId != orderFound.AddressId)
                {
                    orderFound.AddressId = (Guid)order.AddressId;
                }
            }

            return await _orderRepository.UpdateOrderAsync(orderFound);
        }
         private async Task<bool> ValidateIdAsync(Guid id, string entityType)
        {
            bool exists = entityType switch
            {
                "User" => await _userRepository.GetUserByIdAsync(id) != null,
                "Address" => await _addressRepository.GetAddressByIdAsync(id) != null,
                "Product" =>await _productRepo.GetProductByIdAsync(id) != null,
                _ => throw new ArgumentException("Unknown entity type")
            };

            if (!exists)
            {
                throw new ArgumentException($"{entityType} with ID {id} does not exist.");
            }
            return exists;
        }

        
    }
}
