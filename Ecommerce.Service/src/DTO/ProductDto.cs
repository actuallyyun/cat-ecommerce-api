using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ProductReadDto : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int Inventory { get; set; }

        public decimal? Rating { get; set; }
        public IEnumerable<ImageReadDto>? Images { get; set; }
    }

    public class ProductCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        public List<ImageCreateDto>? ImageCreateDto { get; set; }
    }

    public class ProductUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Inventory { get; set; }
        public List<byte[]>? Images { get; set; }
    }
}
