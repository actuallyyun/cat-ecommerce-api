using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("products")]
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Inventory must be greater than or equal to 0")]
        public int Inventory { get; set; }

        [Range(0, int.MaxValue)]
        public decimal? Rating { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
