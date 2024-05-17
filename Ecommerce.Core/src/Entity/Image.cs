using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ecommerce.Core.src.Entity
{
    public class Image : BaseEntity
    {
        [ForeignKey("ProductId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]//ignore nullable fields and do not transfer it to "00000"
        public Guid? ProductId { get; set; }

        [ForeignKey("ReviewId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? ReviewId { get; set; }
        public byte[] Data { get; set; }
    }
}
