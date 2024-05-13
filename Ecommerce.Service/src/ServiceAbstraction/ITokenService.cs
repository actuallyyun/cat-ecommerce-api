
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface ITokenService
    {
        string GenerateToken(User user,TokenType type);
    }
}