using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce.Controller.src.DataModel.FormDataModel;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        public UserController(IUserService userService,IAddressService addressService)
        {
            _userService = userService;
            _addressService=addressService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult<UserReadDto>> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
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
                return await _userService.GetAllUserAsync(options);
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
            return await _userService.GetReviewsByUserIdAsync(id);
        }

        // only admin can get user orders by user id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/orders")]
        public async Task<IEnumerable<OrderReadDto>> GetUserOrdersByUserIdAsync([FromRoute] Guid id)
        {
            return await _userService.GetAllUserOrdersAsync(id);
        }

        // user needs to be logged in to check her own profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User; 
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
            return await _userService.GetReviewsByUserIdAsync(userId);
        }

        [Authorize()] // user can get her own orders
        [HttpGet("orders")]
        public async Task<IEnumerable<OrderReadDto>> ListOrdersAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _userService.GetAllUserOrdersAsync(userId);
        }

                [Authorize()] // user can get her own orders
        [HttpGet("addresses")]
        public async Task<IEnumerable<AddressReadDto>> ListAddressesAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _addressService.GetAllUserAddressesAsync(userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.DeleteUserByIdAsync(id);
        }

    }
}
