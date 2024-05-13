using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.WebApi.src.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user,TokenType type)
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

            //var expires=(type is TokenType.AccessToken)?DateTime.UtcNow.AddMinutes(30):DateTime.UtcNow.AddDays(2);
            
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = securityKey,
                Issuer=_configuration["Secrets:Issuer"],
            };
            var token = tokenHandler.CreateToken(tokenDecriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
