using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        public List<string> Images { get; set; }

        public ProductCreateDto(
            string name,
            string description,
            decimal price,
            Guid categoryId,
            int inventory,
            List<string> images
        )
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            Inventory = inventory;
            Images = images;
        }
    }

    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Inventory { get; set; }
        public List<string>? Images { get; set; }

        public ProductUpdateDto(string? name,string? description,decimal? price,Guid? categoryId,int? inventory,List<string>? images){
                if(name !=null){
                    Name=name;
                }
                if(description !=null){
                    Description=description;
                }
                if(price != null){
                    Price=price;
                }
                if(categoryId != null){
                    CategoryId=categoryId;
                }
                if(inventory != null){
                    Inventory=inventory;
                }
                if(images != null){
                    Images=images;
                }
        }
    }


    public class ProductReadDto
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }
        public Category Category { get; }
        public int Inventory { get; }
        public List<Image> Images { get; }

        public ProductReadDto(
            Guid id,
            string name,
            string description,
            decimal price,
            Category category,
            int inventory,
            List<Image> images
        )
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Inventory = inventory;
            Images = images;
        }
    }
}
