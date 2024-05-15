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
                    var newOrder = new Order
                    {
                        UserId = order.UserId,
                        AddressId = order.AddressId,
                        Status = order.Status
                    };
                    foreach (var item in order.Items)
                    {
                        var foundProduct = await _products.FindAsync(item.ProductId); // ef core tracking
                        var newItem=new OrderItem{
                            ProductId = item.ProductId,
                            OrderId=newOrder.Id,
                            Quantity=item.Quantity,
                            Price=item.Price,
                        };
                        await _orderItems.AddAsync(newItem);
                        foundProduct.Inventory -= item.Quantity;
                    }
                    await _orders.AddAsync(newOrder);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return newOrder;
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
