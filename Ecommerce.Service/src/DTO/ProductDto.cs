namespace Ecommerce.Service.src.DTO
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        public List<ImageCreateDto>? ImageCreateDto { get; set; }

    }

    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Inventory { get; set; }
        public List<byte[]>? Images { get; set; }

    }


}