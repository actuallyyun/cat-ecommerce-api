using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
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

        [Authorize(Roles = "Admin")] // only admin can create
        [HttpPost()]
        public async Task<Product> CreateProductAsync(ProductCreateDto productCreate)
        {
            return await _productService.CreateProductAsync(productCreate);
        }

        [Authorize(Roles = "Admin")] // only admin can update
        [HttpPut("{id}")]
        public async Task<bool> UpdateProductAsync(Guid id, ProductUpdateDto productUpdate)
        {
            return await _productService.UpdateProductByIdAsync(id, productUpdate);
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
    }
}
