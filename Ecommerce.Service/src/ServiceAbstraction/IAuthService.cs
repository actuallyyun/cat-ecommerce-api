using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserCredential credential);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}
