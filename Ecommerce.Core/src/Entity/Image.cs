using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class Image : BaseEntity
    {
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }

        [ForeignKey("ReviewId")]
        public Guid ReviewId { get; set; }
        public byte[] Data { get; set; }
    }
}
