using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("order_items")]
    public class OrderItem : BaseEntity
    {
        public Guid? ProductId { get; set; }

        [Required]
        public Guid OrderId { get; set; } // some how OrderId create a conflic when trying to craete tables using Fluent API

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0.00")]
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public OrderItem() { }// this is needed so DbContext can run
        public OrderItem(Guid? productId, Guid orderId, int quantity, decimal price)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Price = price;
        }
    }
}
