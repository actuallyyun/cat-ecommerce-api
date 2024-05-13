using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService){
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<string> LoginAsync(UserCredential userCredential){
            return await _authService.LoginAsync(userCredential);
        }
    }
}