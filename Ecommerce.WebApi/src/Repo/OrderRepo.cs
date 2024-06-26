using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class OrderRepo : IOrderRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Order> _orders;
        private readonly DbSet<Product> _products;

        private readonly DbSet<OrderItem> _orderItems;

        public OrderRepo(EcommerceDbContext context)
        {
            _context = context;
            _orders = context.Orders;
            _products = context.Products;
            _orderItems = context.OrderItems;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _orders.AddAsync(order);
                    foreach (var item in order.OrderItems)
                    {
                        var foundProduct = await _products.FindAsync(item.ProductId);
                        await _orderItems.AddAsync(item);
                        _products
                            .Where(p => p.Id == item.ProductId)
                            .ExecuteUpdate(setters =>
                                setters.SetProperty(
                                    p => p.Inventory,
                                    p => p.Inventory - item.Quantity
                                )
                            );
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return order;
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
        {
            await _orders.Where(o => o.Id == orderId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions? options)
        {
            var query = new LINQParams(options);

            IEnumerable<Order> orders;

            if (options?.SortOrder == SortOrder.ASC)
            {
                orders = await _orders
                    .Skip(query.SkipFrom)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderBy(r => query.SortBy)
                    .ToListAsync();
            }
            else
            {
                orders = await _orders
                    .Skip(query.SkipFrom)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderByDescending(r => query.SortBy)
                    .ToListAsync();
            }

            return orders;
        }

        public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId)
        {
            return await _orders.Include(order=>order.OrderItems).Include(o=>o.Address).Where(o => o.UserId == userId).ToListAsync(); // ignore queryoptions for now
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
                );
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
