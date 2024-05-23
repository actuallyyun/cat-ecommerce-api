using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Entity
{
    [Table("orders")]
    public class Order : BaseEntity
    {
        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Created;

        // represent one to many relationship with OrderItem
        public List<OrderItem> OrderItems { get; set; }

        // Navigation property
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public User User { get; set; } = null!; //reference
    }
}
