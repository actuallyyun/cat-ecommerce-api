using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce.Controller.src.DataModel.FormDataModel;


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
        public async Task<ActionResult<ProductReadDto>> CreateFromFormAsync(
            [FromForm] ProductForm productForm
        )
        {
            if (productForm == null || productForm.Images == null || productForm.Images.Count == 0)
            {
                return BadRequest("Product data and images are required.");
            }

            var imageList = new List<string>();
            foreach (var image in productForm.Images)
            {
                if (image.Length > 0) //check image size for max length as well
                {
                
                        imageList.Add(image);
                    
                }
            }
            var productCreateDto = new ProductCreateDto
            {
                Inventory = productForm.Inventory,
                Title = productForm.Name,
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
        public async Task<IEnumerable<ReviewReadDto>> ListReviewsAsync([FromRoute] Guid id)
        {
            return await _productService.GetAllReviews(id);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProductByIdAsync(Guid id)
        {
            var res= await _productService.GetProductByIdAsync(id);
            return res==null?NotFound():Ok(res);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<ProductReadDto>> GetAllProductAsync(
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

        
    }
}
