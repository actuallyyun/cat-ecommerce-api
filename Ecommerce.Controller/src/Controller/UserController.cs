using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDemo.Controller.src.Controller
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

            return await _userService.UpdateUserByIdAsync(userId,userUpdate);
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

        // user needs to be logged in to check her own profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User; // not user obbject, but user claims
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _userService.GetUserByIdAsync(userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] 
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            return await _userService.DeleteUserByIdAsync(id);
        }
    }
}
