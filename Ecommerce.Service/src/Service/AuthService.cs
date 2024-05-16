using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public AuthService(
            IUserRepository userRepo,
            ITokenService tokenService,
            IPasswordService passwordService
        )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<string> LoginAsync(UserCredential credential)
        {
            var user =
                await _userRepo.GetUserByCredentialAsync(credential)
                ?? throw new UnauthorizedAccessException("Email is not correct");

            var isValidPassword = _passwordService.VerifyPassword(
                credential.Password,
                user.Password,
                user.Salt
            );

            if (isValidPassword)
            {
                return _tokenService.GenerateToken(user, TokenType.AccessToken);
            }
            else
            {
                throw new UnauthorizedAccessException("Password is not correct.");
            }
        }

        public Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

    }
}
