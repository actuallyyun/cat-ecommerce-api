using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("products")]
    public class Product : BaseEntity
    {


        // Constructor with parameters
        public Product(string name, string description, Category category, decimal price, int inventory)
        {
            Name = name;
            Description = description;
            Price = price;
            Inventory = inventory;
            Category = category;
        }

        // Parameterless constructor for Entity Framework Core
        public Product() { }



        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Inventory must be greater than or equal to 0")]
        public int Inventory { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public void SetProductImages(List<Image> images)
        {
            Images = images;
        }

    }
}
