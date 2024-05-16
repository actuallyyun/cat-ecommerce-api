using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("order_items")]
    public class OrderItem : BaseEntity
    {
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0.00")]
        public decimal Price { get; set; }
    }
}
