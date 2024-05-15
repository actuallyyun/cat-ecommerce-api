using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepositoryAbstraction
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order,List<OrderItem> items);
        Task<bool> UpdateOrderAsync(Order order);
        Task<Order>? GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions? options);
        Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId);
        Task<bool> DeleteOrderByIdAsync(Guid orderId);
    }
}
