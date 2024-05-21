using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class ReviewImage : BaseEntity 
    {
    
        [ForeignKey("ReviewId")]
        public Guid ReviewId { get; set; }
        public Review Review {get;set;}=null!;
        public string Url { get; set; }
    }
}
