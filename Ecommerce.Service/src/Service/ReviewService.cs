using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ReviewService(
            IUserRepository userRepository,
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            IMapper mapper
        )
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Review> CreateReviewAsync(ReviewCreateDto reviewDto)
        {
            await ValidateIdAsync(reviewDto.UserId, "User");
            await ValidateIdAsync(reviewDto.ProductId, "Product");

            var review = _mapper.Map<Review>(reviewDto); // Use AutoMapper to map from DTO to Entity

            foreach (var imageUrl in reviewDto.Images)
            {
                review.Images.Add(new Image(review.Id, imageUrl));
            }

            await _reviewRepository.CreateReviewAsync(review);
            return review;
        }

        public async Task<bool> UpdateReviewByIdAsync(
            Guid userId,
            Guid reviewId,
            ReviewUpdateDto reviewDto
        )
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
                throw new ArgumentException("Review not found.");

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "Unauthorized.You can only update your own reviews."
                );
            }

            _mapper.Map(reviewDto, review);

            return await _reviewRepository.UpdateReviewByIdAsync(review);
        }

        public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync(QueryOptions options)
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync(options);
            return _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }

        public async Task<bool> DeleteReviewByIdAsync(Guid userId, Guid id)
        {
            var existingReview = await _reviewRepository.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                throw new ArgumentException("Review not found with the specified ID.");
            }
            if (id != userId)
            {
                throw new UnauthorizedAccessException(
                    "Unauthorized. You can only delete your own reviews."
                );
            }
            return await _reviewRepository.DeleteReviewByIdAsync(id);
        }

        public async Task<ReviewReadDto> GetReviewByIdAsync(Guid id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
                throw new ArgumentException("Review not found.");

            return _mapper.Map<ReviewReadDto>(review);
        }

        private async Task ValidateIdAsync(Guid id, string entityType)
        {
            bool exists = entityType switch
            {
                "User" => await _userRepository.GetUserByIdAsync(id) != null,
                "Product" => await _productRepository.GetProductByIdAsync(id) != null,
                _ => throw new ArgumentException("Unknown entity type")
            };

            if (!exists)
            {
                throw new ArgumentException($"{entityType} with ID {id} does not exist.");
            }
        }
    }
}
