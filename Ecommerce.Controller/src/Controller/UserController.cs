using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<UserReadDto> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        {
            return await _userService.CreateUserAsync(userCreateDto);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<bool> UpdateUserProfileAsync(UserUpdateDto userUpdate)
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _userService.UpdateUserByIdAsync(userId, userUpdate);
        }

        [Authorize(Roles = "Admin")] // authentication middleware would be invoked if user send get request to this endpoint
        [HttpGet("")] // define endpoint: /users?page=1&pageSize=10
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(
            [FromQuery] QueryOptions options
        )
        {
            try
            {
                return await _userService.GetAllUsersAsync(options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // only admin can get user profile by id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")] // define endpoint: /users/{id}
        public async Task<UserReadDto> GetUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        // only admin can get user reviews by user id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/reviews")]
        public async Task<IEnumerable<Review>> GetUserReviewsByUserIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetAllReviewsAsync(id);
        }

        // only admin can get user orders by user id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/orders")]
        public async Task<IEnumerable<Order>> GetUserOrdersByUserIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetAllOrdersAsync(id);
        }

        // user needs to be logged in to check her own profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _userService.GetUserByIdAsync(userId);
        }

        // user can get her own reviews
        [Authorize()]
        [HttpGet("reviews")]
        public async Task<IEnumerable<Review>> ListAllReviews()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _userService.GetAllReviewsAsync(userId);
        }

        [Authorize()] // user can get her own orders
        [HttpGet("orders")]
        public async Task<IEnumerable<Order>> ListOrdersAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _userService.GetAllOrdersAsync(userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.DeleteUserByIdAsync(id);
        }

        [HttpPost("upload-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UserForm userForm)
        {
            if (userForm.AvatarImage == null || userForm.AvatarImage.Length == 0)
            {
                return BadRequest("Avatar is missing");
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    await userForm.AvatarImage.CopyToAsync(ms);
                    var content = ms.ToArray();
                    // return File(content, userForm.AvatarImage.ContentType);
                    return File(content, userForm.AvatarImage.ContentType);
                }
            }
        }

        public class UserForm
        {
            public IFormFile AvatarImage { get; set; }
            public Guid UserId { get; set; }
        }
    }
}
