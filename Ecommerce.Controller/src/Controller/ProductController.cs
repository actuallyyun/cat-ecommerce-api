using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")] 
        [HttpPost()]
        public async Task<ActionResult<Product>> CreateFromFormAsync(
            [FromForm] ProductForm productForm
        )
        {
            if (productForm == null || productForm.Images == null || productForm.Images.Count == 0)
            {
                return BadRequest("Product data and images are required.");
            }

            var imageList = new List<ImageCreateDto>();
            foreach (var image in productForm.Images)
            {
                if (image.Length > 0) //check image size for max length as well
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        imageList.Add(new ImageCreateDto(ms.ToArray()));
                    }
                }
            }
            var productCreateDto = new ProductCreateDto
            {
                Inventory = productForm.Inventory,
                Name = productForm.Name,
                Description = productForm.Description,
                Price = productForm.Price,
                ImageCreateDto = imageList,
                CategoryId=productForm.CategoryId,
            };
            return await _productService.CreateProductAsync(productCreateDto);
        }

        [Authorize(Roles = "Admin")] // only admin can update
        [HttpPut("{id}")]
        public async Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto productUpdate)
        {
            return await _productService.UpdateProductByIdAsync(id, productUpdate);
        }

        [AllowAnonymous]
        [HttpGet("{id}/reviews")]
        public async Task<IEnumerable<Review>> ListReviewsAsync([FromRoute] Guid id)
        {
            return await _productService.GetAllReviews(id);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productService.GetProductByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<Product>> GetAllProductAsync(
            [FromQuery] QueryOptions queryOptions
        )
        {
            return await _productService.GetAllProductsAsync(queryOptions);
        }

        [Authorize(Roles = "Admin")] // only admin can delete
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid id)
        {
            return await _productService.DeleteProductByIdAsync(id);
        }

        public class ProductForm
        {
            public int Inventory { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }

            public Guid CategoryId{get;set;}
            public List<IFormFile> Images { get; set; }
        }
    }
}
