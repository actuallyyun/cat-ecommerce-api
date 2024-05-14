using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IReviewService
    {
        Task<Review> CreateReviewAsync(ReviewCreateDto review);
        Task<bool> UpdateReviewByIdAsync(Guid userId,Guid reviewId,ReviewUpdateDto review);
        Task<Review>? GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetAllReviewsAsync(QueryOptions options);
        Task<bool> DeleteReviewByIdAsync(Guid userId,Guid id);
        
    }
}
