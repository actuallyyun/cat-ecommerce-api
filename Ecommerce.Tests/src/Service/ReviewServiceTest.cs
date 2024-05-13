
using Moq;
using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Tests.src.Service
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _mockReviewRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            // Initialize the mock repositories and AutoMapper
            _mockReviewRepo = new Mock<IReviewRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();

            _reviewService = new ReviewService(_mockUserRepo.Object, _mockProductRepo.Object, _mockReviewRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateReviewAsync_WithValidData_ShouldCreateAndReturnReview()
        {
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var category = new Category("Electronics", "http://example.com/category_image.jpg");
            var user = new User("Jane", "Doe", UserRole.User, "avatar-url.jpg", "jane.doe@example.com", "password123");
            var product = new Product("Laptop", "High performance laptop", category, 1200.00M, 10);
            
            var reviewDto = new ReviewCreateDto
            {
                UserId = userId,
                ProductId = productId,
                IsAnonymous = false,
                Content = "Excellent product!",
                Rating = 5,
                Images = new List<string> { "http://example.com/image1.jpg" }
            };

            var expectedReview = new Review(
                reviewDto.UserId,
                reviewDto.ProductId,
                reviewDto.IsAnonymous,
                reviewDto.Content,
                reviewDto.Rating
            );

            _mockMapper.Setup(m => m.Map<Review>(reviewDto)).Returns(expectedReview);
            _mockUserRepo.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mockProductRepo.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(product);
            _mockReviewRepo.Setup(x => x.CreateReviewAsync(It.IsAny<Review>())).ReturnsAsync(expectedReview);

            var result = await _reviewService.CreateReviewAsync(reviewDto);

            Assert.NotNull(result);
            Assert.Equal(reviewDto.UserId, result.UserId);
            Assert.Equal(reviewDto.ProductId, result.ProductId);
            Assert.Equal(reviewDto.IsAnonymous, result.IsAnonymous);
            Assert.Equal(reviewDto.Content, result.Content);
            Assert.Equal(reviewDto.Rating, result.Rating);

            _mockUserRepo.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _mockProductRepo.Verify(x => x.GetProductByIdAsync(productId), Times.Once);
            _mockReviewRepo.Verify(x => x.CreateReviewAsync(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public async Task CreateReviewAsync_WithInvalidUserId_ShouldThrowArgumentException()
        {
            var invalidUserId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var reviewDto = new ReviewCreateDto
            {
                UserId = invalidUserId,
                ProductId = productId,
                IsAnonymous = false,
                Content = "Invalid user test",
                Rating = 3,
                Images = new List<string> { "http://example.com/image1.jpg" }
            };

            _mockUserRepo.Setup(x => x.GetUserByIdAsync(invalidUserId)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.CreateReviewAsync(reviewDto));
            _mockUserRepo.Verify(x => x.GetUserByIdAsync(invalidUserId), Times.Once);
        }

        [Fact]
        public async Task CreateReviewAsync_WithInvalidProductId_ShouldThrowArgumentException()
        {
            var userId = Guid.NewGuid();
            var invalidProductId = Guid.NewGuid();
            var reviewDto = new ReviewCreateDto
            {
                UserId = userId,
                ProductId = invalidProductId,
                IsAnonymous = false,
                Content = "Invalid product test",
                Rating = 3,
                Images = new List<string> { "http://example.com/image1.jpg" }
            };

            _mockUserRepo.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(new User("Jane", "Doe", UserRole.User, "avatar-url.jpg", "jane.doe@example.com", "password123"));
            _mockProductRepo.Setup(x => x.GetProductByIdAsync(invalidProductId)).ReturnsAsync((Product)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.CreateReviewAsync(reviewDto));
            _mockProductRepo.Verify(x => x.GetProductByIdAsync(invalidProductId), Times.Once);
        }

        [Fact]
        public async Task UpdateReviewByIdAsync_WithValidId_ShouldUpdateReview()
        {
            var reviewId = Guid.NewGuid();
            var updatedContent = "Updated review content";
            var updatedRating = 4;
            var reviewDto = new ReviewUpdateDto { Content = updatedContent, Rating = updatedRating };
            var existingReview = new Review(reviewId, Guid.NewGuid(), false, "Initial review content", 3);

            _mockReviewRepo.Setup(r => r.GetReviewByIdAsync(reviewId)).ReturnsAsync(existingReview);
            _mockReviewRepo.Setup(r => r.UpdateReviewByIdAsync(It.IsAny<Review>())).ReturnsAsync(true);

            _mockMapper.Setup(m => m.Map(reviewDto, existingReview))
                .Callback<ReviewUpdateDto, Review>((src, dest) =>
                {
                    if (src.Content != null)
                    {
                        dest.Content = src.Content;
                    }
                    if (src.Rating.HasValue)
                    {
                        dest.Rating = src.Rating.Value;
                    }
                });


            var result = await _reviewService.UpdateReviewByIdAsync(reviewId,reviewId, reviewDto);

            Assert.True(result);
            Assert.Equal(updatedContent, existingReview.Content);
            Assert.Equal(updatedRating, existingReview.Rating);
            _mockReviewRepo.Verify(r => r.GetReviewByIdAsync(reviewId), Times.Once);
            _mockReviewRepo.Verify(r => r.UpdateReviewByIdAsync(existingReview), Times.Once);
        }

        [Fact]
        public async Task UpdateReviewByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            var invalidReviewId = Guid.NewGuid();
            var reviewDto = new ReviewUpdateDto { Content = "Nonexistent review", Rating = 2 };

            _mockReviewRepo.Setup(r => r.GetReviewByIdAsync(invalidReviewId)).ReturnsAsync((Review)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.UpdateReviewByIdAsync(invalidReviewId,invalidReviewId, reviewDto));
            _mockReviewRepo.Verify(r => r.GetReviewByIdAsync(invalidReviewId), Times.Once);
        }

        [Fact]
        public async Task GetAllReviewsAsync_ShouldReturnAllReviews()
        {
            var queryOptions = new QueryOptions { Limit = 10 };
            var reviews = new List<Review>
            {
                new Review(Guid.NewGuid(), Guid.NewGuid(), false, "Review 1", 5),
                new Review(Guid.NewGuid(), Guid.NewGuid(), true, "Review 2", 4)
            };
            var reviewDtos = reviews.ConvertAll(r => new ReviewReadDto(r.Id, r.UserId, r.ProductId, r.IsAnonymous, r.Content, r.Rating, new List<Image>()));

            _mockReviewRepo.Setup(r => r.GetAllReviewsAsync(queryOptions)).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewReadDto>>(reviews)).Returns(reviewDtos);

            var result = await _reviewService.GetAllReviewsAsync(queryOptions);

            Assert.Equal(2, result.Count());
            _mockReviewRepo.Verify(r => r.GetAllReviewsAsync(queryOptions), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<ReviewReadDto>>(reviews), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewByIdAsync_ShouldReturnTrue_WhenReviewExists()
        {
            var reviewId = Guid.NewGuid();
            var existingReview = new Review(Guid.NewGuid(), Guid.NewGuid(), false, "Review to delete", 5);

            _mockReviewRepo.Setup(r => r.GetReviewByIdAsync(reviewId)).ReturnsAsync(existingReview);
            _mockReviewRepo.Setup(r => r.DeleteReviewByIdAsync(reviewId)).ReturnsAsync(true);

            var result = await _reviewService.DeleteReviewByIdAsync(reviewId,reviewId);

            Assert.True(result);
            _mockReviewRepo.Verify(r => r.GetReviewByIdAsync(reviewId), Times.Once);
            _mockReviewRepo.Verify(r => r.DeleteReviewByIdAsync(reviewId), Times.Once);
        }

        [Fact]
        public async Task DeleteReviewByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            var invalidReviewId = Guid.NewGuid();

            _mockReviewRepo.Setup(r => r.GetReviewByIdAsync(invalidReviewId)).ReturnsAsync((Review)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.DeleteReviewByIdAsync(invalidReviewId,invalidReviewId));
            _mockReviewRepo.Verify(r => r.GetReviewByIdAsync(invalidReviewId), Times.Once);
        }

        [Fact]
        public async Task GetReviewByIdAsync_ShouldReturnReview_WhenReviewExists()
        {
            var reviewId = Guid.NewGuid();
            var existingReview = new Review(reviewId, Guid.NewGuid(), false, "Sample review content", 4);
            var reviewReadDto = new ReviewReadDto(reviewId, existingReview.UserId, existingReview.ProductId, existingReview.IsAnonymous, existingReview.Content, existingReview.Rating, new List<Image>());

            _mockReviewRepo.Setup(r => r.GetReviewByIdAsync(reviewId)).ReturnsAsync(existingReview);
            _mockMapper.Setup(m => m.Map<ReviewReadDto>(existingReview)).Returns(reviewReadDto);

            var result = await _reviewService.GetReviewByIdAsync(reviewId);

            Assert.NotNull(result);
            Assert.Equal(reviewReadDto.Id, result.Id);
            _mockReviewRepo.Verify(r => r.GetReviewByIdAsync(reviewId), Times.Once);
            _mockMapper.Verify(m => m.Map<ReviewReadDto>(existingReview), Times.Once);
        }

    }
}
