using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    [Table("images")]
    public class Image : BaseEntity
    {
        public Guid EntityId { get; set; }

        public string Url { get; set; }

        // Add a parameterless constructor for Entity Framework
        public Image() { }

        public Image(Guid entityId, string url)
        {
            EntityId = entityId;
            Url = url;
        }
    }
}
