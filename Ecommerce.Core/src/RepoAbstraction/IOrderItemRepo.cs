using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IOrderItemRepo
    {
        Task<OrderItem> CreateAsync(OrderItem item);
        Task<bool> DeleteOrderByIdAsync(Guid id
        );
    }
}
