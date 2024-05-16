using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("addresses")]
    public class Address : BaseEntity
    {
        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(128)]
        public string Country { get; set; }

        [MaxLength(40)]
        public string PhoneNumber { get; set; }
    }
}
