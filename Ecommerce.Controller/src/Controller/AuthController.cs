using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using static Ecommerce.Service.src.DTO.TokenDto;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ResponseTokenReadDto> LoginAsync(UserCredential userCredential)
        {
            return await _authService.LoginAsync(userCredential);
        }

        [HttpPost("logout")]
        public async Task<bool> LogoutAsync()
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _authService.LogoutAsync(userId);
        }

        [HttpPost("refresh-token")]
        public async Task<ResponseTokenReadDto> RefreshTokenAsync([FromBody] RefreshTokenDto refreshToken)
        {    
            return await _authService.RefreshTokenAsync(refreshToken.RefreshToken);
        }
    }
}
