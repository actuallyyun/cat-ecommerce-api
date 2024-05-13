using Ecommerce.Core.src.Entity;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public string Image { get; set; }
}


public class CategoryUpdateDto
{
    public string Name { get; set; }
    public string Image { get; set; }

}

public class CategoryReadDto
{
    public Guid Id { get; }
    public string Name { get; }
    public string Image { get; }

    public CategoryReadDto(Guid id, string name, string image)

    {
        Id = id;
        Name = name;
        Image = image;
    }
}

