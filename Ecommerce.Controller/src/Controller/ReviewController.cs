using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<Review>> CreateReview(ReviewCreateDto reviewCreate)
        {
            return await _reviewService.CreateReviewAsync(reviewCreate);
        }

        [Authorize(Roles = "Admin")] // only admin can update reivews
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateReview(
            [FromRoute] Guid id,
            ReviewUpdateDto reviewUpdate
        )
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _reviewService.UpdateReviewByIdAsync(userId, id, reviewUpdate);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<Review> RetrieveSingleReview([FromRoute] Guid id)
        {
            return await _reviewService.GetReviewByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<Review>> ListReviews([FromQuery] QueryOptions queryOptions)
        {
            return await _reviewService.GetAllReviewsAsync(queryOptions);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteReview([FromRoute] Guid id)
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _reviewService.DeleteReviewByIdAsync(userId, id);
        }
    }
}
