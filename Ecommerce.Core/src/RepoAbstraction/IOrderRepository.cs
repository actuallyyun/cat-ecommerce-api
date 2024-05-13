using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepositoryAbstraction
{
  public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<Order>? GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId, QueryOptions? options);
        Task<bool> DeleteOrderByIdAsync(Guid orderId);
    }
  }