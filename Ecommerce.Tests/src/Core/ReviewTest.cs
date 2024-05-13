
using Ecommerce.Core.src.Entity;

namespace  Ecommerce.Tests.src.Core
{
    public class ReviewTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void Rating_SetValidRating_ShouldSetRating(int rating)
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var isAnonymous = false;
            var content = "This is a review.";
            
            var review = new Review(userId, productId, isAnonymous, content, rating);

            Assert.Equal(rating, review.Rating);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void Rating_SetInvalidRating_ShouldThrowArgumentOutOfRangeException(int rating)
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var isAnonymous = false;
            var content = "This is a review.";
            
            Assert.Throws<ArgumentOutOfRangeException>(() => new Review(userId, productId, isAnonymous, content, rating));
        }

        [Fact]
        public void Images_InitializedWithEmptyList_ShouldNotBeNull()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var isAnonymous = false;
            var content = "This is a review.";
            var rating = 4;
            
            var review = new Review(userId, productId, isAnonymous, content, rating);

            Assert.NotNull(review.Images);
        }

        [Fact]
        public void Images_AddImageToList_ShouldIncreaseListCount()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var isAnonymous = false;
            var content = "This is a review.";
            var rating = 4;
            var image = new Image(userId,"imageurl");

            var review = new Review(userId, productId, isAnonymous, content, rating);
            review.Images.Add(image);

            Assert.Single(review.Images);
        }

        [Fact]
        public void IsAnonymous_SetToTrue_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var isAnonymous = true;
            var content = "This is an anonymous review.";
            var rating = 3;

            var review = new Review(userId, productId, isAnonymous, content, rating);

            Assert.True(review.IsAnonymous);
        }
    }
}
