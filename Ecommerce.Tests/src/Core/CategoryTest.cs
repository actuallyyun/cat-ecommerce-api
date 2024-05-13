using Ecommerce.Core.src.Entity;

namespace Ecommerce.Tests.src.Core
{
    public class CategoryTests
    {
        [Fact]
        public void Name_SetName_ShouldSetName()
        {
            var category = new Category("Electronics", "");
            var expectedName = "Electronics";

            category.Name = expectedName;

            Assert.Equal(expectedName, category.Name);
        }

        [Fact]
        public void Image_SetImage_ShouldSetImage()
        {
            var category = new Category("Electronics", "");
            var imageUrl = "http://example.com/image.jpg";

            category.Image = imageUrl;

            Assert.Equal(imageUrl, category.Image);
        }

        [Fact]
        public void Image_ModifyImage_ShouldReflectChanges()
        {
            var category = new Category("Electronics", "");
            var initialImageUrl = "http://example.com/initial.jpg";
            var updatedImageUrl = "http://example.com/updated.jpg";

            category.Image = initialImageUrl;
            category.Image = updatedImageUrl;

            Assert.Equal(updatedImageUrl, category.Image);
        }

        [Fact]
        public void Constructor_WhenCalled_ShouldInitializePropertiesCorrectly()
        {
            var imageUrl = "http://example.com/image.jpg";

            var category = new Category("Fashion", imageUrl);
            
            Assert.Equal("Fashion", category.Name);
            Assert.Equal(imageUrl, category.Image);
        }
    }
}
