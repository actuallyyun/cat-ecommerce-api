using Microsoft.AspNetCore.Http;

namespace Ecommerce.Controller.src.DataModel
{
    public class FormDataModel
    {
        public class ProductForm
        {
            public int Inventory { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }

            public Guid CategoryId { get; set; }
            public List<IFormFile> Images { get; set; }
        }

        public class ReviewForm
        {
            public Guid ProductId { get; set; }
            public bool IsAnonymous { get; set; }
            public string Content { get; set; }
            public int Rating { get; set; }
            public List<IFormFile> Images { get; set; }
        }
    }
}
