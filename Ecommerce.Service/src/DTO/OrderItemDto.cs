namespace Ecommerce.Service.src.DTO
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public OrderItemDto(Guid productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}
