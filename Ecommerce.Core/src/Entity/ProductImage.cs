using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class ProductImage : BaseEntity
    {
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }
        public string Url { get; set; }
    }
}
