using System.Data.Common;
using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class ReviewRepo : IReviewRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Review> _reviews;

        public ReviewRepo(EcommerceDbContext context)
        {
            _context = context;
            _reviews = _context.Reviews;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            await _reviews.AddAsync(review);

            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewByIdAsync(Guid id)
        {
            await _reviews.Where(r => r.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync(QueryOptions? options)
        {
            var query = new LINQParams(options);

            IEnumerable<Review> reviews;

            if (options?.SortOrder == SortOrder.ASC)
            {
                reviews = await _reviews
                    .Where(r => r.Content.Contains(query.SearchKey))
                    .Skip(query.SkipFrom)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderBy(r => query.SortBy)
                    .ToListAsync();
            }
            else
            {
                reviews = await _reviews
                    .Where(r => r.Content.Contains(query.SearchKey))
                    .Skip(query.SkipFrom)
                    .Take(options?.Limit ?? AppConstants.PER_PAGE)
                    .OrderByDescending(r => query.SortBy)
                    .ToListAsync();
            }

            return Enumerable.Cast<Review>(reviews);
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            var review = await _reviews.SingleOrDefaultAsync(r => r.Id == id);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {id} not found.");
            }
            return review;
        }

        public async Task<bool> UpdateReviewByIdAsync(Review review)
        {
            await _reviews
                .Where(r => r.Id == review.Id)
                .ExecuteUpdateAsync(setters =>
                    setters
                        .SetProperty(r => r.IsAnonymous, review.IsAnonymous)
                        .SetProperty(r => r.Content, review.Content)
                        .SetProperty(r => r.Rating, review.Rating)
                );

            return true;
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId)
        {
            return await _reviews
                .Include(review => review.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _reviews.Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
