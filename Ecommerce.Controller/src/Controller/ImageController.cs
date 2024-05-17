using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService service)
        {
            _imageService = service;
        }

        [HttpGet("get-file/{id}")]
        public async Task<FileContentResult> GetFile(Guid id)
        {
            var file = await _imageService.GetByIdAsync(id);
            return File(file.Data, "image/jpeg");
        }
    }
}
