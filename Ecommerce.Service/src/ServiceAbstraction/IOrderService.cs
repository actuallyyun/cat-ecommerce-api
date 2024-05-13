using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderCreateDto order);
        Task<bool> UpdateOrderByIdAsync(Guid id, OrderUpdateDto order);
        Task<Order>? GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId, QueryOptions? options);
        Task<bool> DeleteOrderByIdAsync(Guid id);
    }
}
