using Ecommerce.Core.src.Common;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(QueryOptions options);
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<Review> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewByIdAsync(Review review);
        Task<bool> DeleteReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);
    }
}