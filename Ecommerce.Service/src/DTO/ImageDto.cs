
namespace Ecommerce.Service.src.DTO
{
    public class ImageCreateDto
    {
        public string Url { get; set; }
    }

    public class ImageReadDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
    }


}