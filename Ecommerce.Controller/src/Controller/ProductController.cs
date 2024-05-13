using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        // should be admin and superadmin only
        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreate)
        {
            return await _productService.CreateProductAsync(productCreate);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto productUpdate)
        {
            return await _productService.UpdateProductByIdAsync(id, productUpdate);
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<ProductReadDto> GetProductByIdAsync(Guid id)
        {
            return await _productService.GetProductByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<ProductReadDto>> GetAllProductAsync(
            [FromQuery] QueryOptions queryOptions
        )
        {
            return await _productService.GetAllProductsAsync(queryOptions);
        }

        [AllowAnonymous]
        [HttpDelete("id")]
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid id)
        {
            return await _productService.DeleteProductByIdAsync(id);
        }
    }
}
