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
        public User User{get;set;}=null!;

        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(40)]
        public string LastName { get; set; }

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
