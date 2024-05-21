using System.Data;
using System.Security.Claims;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IAuthorizationService _authService;

        public AddressController(
            IAddressService addressService,
            IAuthorizationService authorizationService
        )
        {
            _addressService = addressService;
            _authService = authorizationService;
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<Address>> CreateAddressAsync(
            [FromBody] AddressCreateDto addressCreate
        )
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            addressCreate.UserId = userId;
            return await _addressService.CreateAddressAsync(addressCreate);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateAddressAsync(
            Guid id,
            AddressUpdateDto addressUpdate
        )
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

                return await _addressService.UpdateAddressByIdAsync(id, userId, addressUpdate);
            }
        

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<bool>> RetrieveAddressAsync([FromRoute] Guid id)
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var res = await _addressService.GetAddressByIdAsync(id, userId);
            return res == null ? NotFound() : Ok(res);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IEnumerable<Address>> ListAddressByUserId()
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _addressService.GetAllUserAddressesAsync(userId);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAddressAsync([FromRoute] Guid id)
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return await _addressService.DeleteAddressByIdAsync(id, userId);
        }
    }
}
