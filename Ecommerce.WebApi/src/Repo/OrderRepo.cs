using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class OrderRepo : IOrderRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Order> _orders;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
        {
            await _orders.Where(o => o.Id == orderId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(
            Guid userId,
            QueryOptions? options
        )
        {
            return await _orders.Where(o => o.UserId == userId).ToListAsync(); // ignore queryoptions for now
        }

        public async Task<Order>? GetOrderByIdAsync(Guid orderId)
        {
            return await _orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            await _orders
                .Where(o => o.Id == order.Id)
                .ExecuteUpdateAsync(setters =>
                    setters
                        .SetProperty(u => u.Address, order.Address)
                        .SetProperty(u => u.Status, order.Status)
                        .SetProperty(u => u.UpdatedAt, DateTime.Now)
                );
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
