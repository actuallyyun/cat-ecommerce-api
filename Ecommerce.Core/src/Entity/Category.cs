using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Core.src.Entity
{
    [Table("categories")]
    public class Category : BaseEntity
    {
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public string Image { get; set; } = AppConstants.CATEGORY_DEFAULT_IMAGE;

        public Category() { }
        public Category(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }


}
