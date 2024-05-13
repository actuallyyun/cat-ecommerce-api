using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("addresses")]
    public class Address : BaseEntity
    {
        [Required]
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

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        public Address() { }

        public Address(Guid userId, string addressLine, string postalCode, string country, string phoneNumber)
        {
            UserId = userId;
            AddressLine = addressLine;
            PostalCode = postalCode;
            Country = country;
            PhoneNumber = phoneNumber;
        }
    }
}
