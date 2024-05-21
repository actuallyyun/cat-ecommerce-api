using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.WebApi.src.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private  readonly IRefreshTokenRepo _refreshTokenRepo;

        private  readonly IUserRepository _userRepository;
     

        public TokenService(IConfiguration configuration,IRefreshTokenRepo refreshToken,IUserRepository userRepo)
        {
            _configuration = configuration;
            _refreshTokenRepo=refreshToken;
            _userRepository=userRepo;
        }

        public async Task<ResponseToken> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var jwtKey = _configuration["Secrets:JwtKey"];

            if (jwtKey is null)
            {
                throw new ArgumentNullException(
                    "Jwtkey is not found. Check if you have it in appsettings.json"
                );
            }

            var securityKey = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = securityKey,
                Issuer = _configuration["Secrets:Issuer"],
            };
            var accessToken = tokenHandler.CreateToken(tokenDecriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            var refreshToken = GenerateRefreshToken(user.Id);

            await _refreshTokenRepo.CreateAsync(refreshToken);

            return new ResponseToken{
                AccessToken=accessTokenString,
                RefreshToken=refreshToken.Token
            };

        }

        private static RefreshToken GenerateRefreshToken(Guid userId)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomBytes),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<ResponseToken> RefreshToken(string refreshToken)
        {
            var resfreshTokenInStorage=await _refreshTokenRepo.GetAsync(refreshToken);
   
            if (resfreshTokenInStorage == null || resfreshTokenInStorage.IsRevoked )
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            if (resfreshTokenInStorage.ExpiresAt < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token has expired");
            }

            var user=await _userRepository.GetUserByIdAsync(resfreshTokenInStorage.UserId);

            var newToken=await GenerateToken(user);

            // update the refresh token
            await _refreshTokenRepo.UpdateAsync(user.Id,newToken.RefreshToken);

            return newToken;
        }

        public async Task<bool> InvalidateTokenAsync(Guid id)
        {
            var refreshTokenKey = await _refreshTokenRepo.GetByUserIdAsync(id);
            if (refreshTokenKey != null)
            {
                refreshTokenKey.IsRevoked=true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
