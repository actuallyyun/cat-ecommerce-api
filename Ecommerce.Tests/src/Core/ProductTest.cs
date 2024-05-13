using Ecommerce.Core.src.Entity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Tests.src.Core
{
    public class ProductTests
    {

        private void ValidateProduct(Product product)
        {
            var validationContext = new ValidationContext(product);
            Validator.ValidateObject(product, validationContext, validateAllProperties: true);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var name = "Smartphone";
            var description = "A new smartphone";
            var category = new Category("Electronics", "http://example.com/electronics.jpg");
            var price = 499.99m;
            var inventory = 100;

            var product = new Product(name, description, category, price, inventory);

            Assert.Equal(name, product.Name);
            Assert.Equal(description, product.Description);
            Assert.Equal(category, product.Category);
            Assert.Equal(price, product.Price);
            Assert.Equal(inventory, product.Inventory);
            Assert.NotNull(product.Images);
        }

        [Fact]
        public void AddProductImages_ShouldAddSuccessfully()
        {
            var name = "Smartphone";
            var description = "A new smartphone";
            var category = new Category("Electronics", "http://example.com/electronics.jpg");
            var price = 499.99m;
            var inventory = 100;

            var product = new Product(name, description, category, price, inventory);

            var image = new Image(Guid.NewGuid(), "http://example.com/smartphone.jpg");
            product.Images.Add(image);

            Assert.Contains(image, product.Images);
        }

        [Fact]
        public void AddMultipleProductImages_ShouldIncreaseImageCount()
        {
            var name = "Smartphone";
            var description = "A new smartphone";
            var category = new Category("Electronics", "http://example.com/electronics.jpg");
            var price = 499.99m;
            var inventory = 100;

            var product = new Product(name, description, category, price, inventory);

            var images = new List<Image>
            {
                new Image(Guid.NewGuid(), "http://example.com/image1.jpg"),
                new Image(Guid.NewGuid(), "http://example.com/image2.jpg"),
                new Image(Guid.NewGuid(), "http://example.com/image3.jpg")
            };

            product.SetProductImages(images);

            Assert.Equal(3, product.Images.Count);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowValidationException()
        {
            var description = "A new smartphone";
            var category = new Category("Electronics", "http://example.com/electronics.jpg");
            var price = 499.99m;
            var inventory = 100;

            var product = new Product(string.Empty, description, category, price, inventory);

            Assert.Throws<ValidationException>(() => ValidateProduct(product));
        }


        [Fact]
        public void UpdateProductDetails_ShouldUpdateCorrectly()
        {
            var name = "Smartphone";
            var description = "A new smartphone";
            var category = new Category("Electronics", "http://example.com/electronics.jpg");
            var price = 499.99m;
            var inventory = 100;

            var product = new Product(name, description, category, price, inventory);

            // Update details
            var newName = "Updated Smartphone";
            var newDescription = "An updated smartphone";
            product.Name = newName;
            product.Description = newDescription;

            Assert.Equal(newName, product.Name);
            Assert.Equal(newDescription, product.Description);
        }
    }
}
