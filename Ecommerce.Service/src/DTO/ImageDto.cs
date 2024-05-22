
namespace Ecommerce.Service.src.DTO
{
    public class ImageCreateDto
    {
        public string Url { get; set; }
        public ImageCreateDto(string url){
            Url=url;
        }
    }

    public class ImageReadDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
    }


}