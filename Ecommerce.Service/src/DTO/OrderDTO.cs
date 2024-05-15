using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public List<OrderItemDto> Items { get; set; }

        public OrderCreateDto(
            Guid userId,
            Guid addressId,
            List<OrderItemDto> items
        )
        {
            UserId = userId;
            AddressId = addressId;
            Items = items;
        }
    }

    public class OrderUpdateDto
    {
        public Guid? AddressId { get; set; }
        public OrderStatus? Status { get; set; }

        public  OrderUpdateDto(Guid? addressId, OrderStatus? orderStatus)
        {
            if (addressId != null)
            {
                AddressId = addressId;
            }
            if (orderStatus != null)
            {
                Status = orderStatus;
            }
        }
    }
}
