using Ecommerce.Core.src.Common;
using static Ecommerce.Service.src.DTO.TokenDto;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IAuthService
    {
        Task<ResponseTokenReadDto> LoginAsync(UserCredential credential);
        Task<ResponseTokenReadDto> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(Guid userId);
    }
}
