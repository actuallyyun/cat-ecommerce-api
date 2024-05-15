
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class OrderItemRepo : IOrderItemRepo
    {

        private readonly EcommerceDbContext _context;
        private readonly DbSet<OrderItem> _items;

        public OrderItemRepo(EcommerceDbContext context){
            _context=context;
            _items=context.OrderItems;
        }

        public async Task<OrderItem> CreateAsync(OrderItem item)
        {
            await _items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid id)
        {
            await _items.Where(i=>i.Id==id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}