using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstraction
{
    public interface IRefreshTokenRepo
    {
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
        Task<bool> UpdateAsync(Guid userId,string token);
        Task<RefreshToken>? GetAsync(string token);
        Task<RefreshToken>? GetByUserIdAsync(Guid id);
        Task<bool> DeleteAsync(RefreshToken refreshToken);
    }
}
