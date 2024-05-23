using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IOrderService
    {
        Task<OrderReadDto> CreateOrderAsync(OrderCreateDto order);
        Task<bool> UpdateOrderByIdAsync(Guid id, OrderUpdateDto order);
        Task<OrderReadDto> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(QueryOptions? options);
        Task<IEnumerable<OrderReadDto>> GetAllOrdersByUserAsync(Guid userId);
        Task<bool> DeleteOrderByIdAsync(Guid id);
    }
}
