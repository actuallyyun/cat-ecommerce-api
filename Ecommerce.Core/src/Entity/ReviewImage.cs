using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class ReviewImage:Image
    {
   
        public Guid UserId { get; set; } // Foreign key navigate to product
        public Review Review { get; set; } = null!;

        //public ReviewImage(Guid id,Byte[] data):base(data){
        //    ReviewId=id;
        //}
    }
    
}