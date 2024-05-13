using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public List<OrderItemDto> ItemsDto { get; set; }

        public OrderCreateDto(
            Guid userId,
            Guid addressId,
            OrderStatus status,
            List<OrderItemDto> itemsDto
        )
        {
            UserId = userId;
            AddressId = addressId;
            Status = status;
            ItemsDto = itemsDto;
        }
    }

    public class OrderUpdateDto
    {
        public Guid? AddressId { get; set; }
        public OrderStatus? Status { get; set; }

        public  OrderUpdateDto(Guid? addressId, OrderStatus orderStatus)
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
