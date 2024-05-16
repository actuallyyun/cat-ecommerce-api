using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public List<OrderItemCreateDto> OrderItemCreateDto { get; set; }
    }

    public class OrderItemCreateDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public class OrderUpdateDto
    {
        public Guid? AddressId { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
