
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface ITokenService
    {
        Task<ResponseToken> GenerateToken(User user);
        Task<ResponseToken> RefreshToken(string refreshToken);
        Task<bool> InvalidateTokenAsync(Guid id);
    }
}