using System.Security.Claims;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/reiviews")]
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
            try
            {
                var review = await _reviewService.CreateReviewAsync(reviewCreate);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("id")]
        public async Task<ActionResult<bool>> UpdateReview(
            [FromRoute] Guid id,
            ReviewUpdateDto reviewUpdate
        )
        {
            var claims = HttpContext.User;

            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                var res = await _reviewService.UpdateReviewByIdAsync(userId, id, reviewUpdate);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<ActionResult<Review>> RetrieveSingleReview([FromRoute] Guid id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("id")]
        public async Task<ActionResult<bool>> DeleteReview([FromRoute] Guid id)
        {
            var claims = HttpContext.User;

            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                await _reviewService.DeleteReviewByIdAsync(userId, id);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
