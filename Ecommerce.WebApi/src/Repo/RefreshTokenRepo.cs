using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<RefreshToken> _refreshTokens;

        public RefreshTokenRepo(EcommerceDbContext context)
        {
            _context = context;
            _refreshTokens = _context.RefreshTokens;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            await _refreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<bool> DeleteAsync(RefreshToken refreshToken)
        {
            await _refreshTokens.Where(r => r.Id == refreshToken.Id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RefreshToken>? GetAsync(string token)
        {
            return await _refreshTokens.FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<RefreshToken>? GetByUserIdAsync(Guid id)
        {
            return await _refreshTokens.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> UpdateAsync(Guid userId, string token)
        {
            await _refreshTokens
                .Where(r => r.UserId == userId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(r => r.Token, token).SetProperty(r => r.IsRevoked, false)
                );
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
