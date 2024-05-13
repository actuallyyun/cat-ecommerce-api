using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Entity
{
    [Table("orders")]
    public class Order : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; } // net set so I can set id and add dummy data

        [Required]
        public Guid AddressId { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Created;

        // represent one to many relationship with OrderItem
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Navigation property
        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Order() { }// this is needed so the DbContext can run
        public Order(Guid userId, Guid addressId, OrderStatus status)
        {
            UserId = userId;
            AddressId = addressId;
            Status = status;
        }
    }
}
