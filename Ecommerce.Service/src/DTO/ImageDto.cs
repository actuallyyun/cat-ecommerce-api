
namespace Ecommerce.Service.src.DTO
{
    public class ImageCreateDto
    {
        public byte[] Data { get; set; }
        public ImageCreateDto(byte[] data){
            Data=data;
        }
    }

    public class ImageReadDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ImageUpdateDto
    {
        public byte[] Data { get; set; }
    }

}