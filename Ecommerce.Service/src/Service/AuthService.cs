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
        private User _currentUser;

        public AuthService(IUserRepository userRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        private void SetCurrentUser(User user)
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
        }

        public async Task<string> LoginAsync(UserCredential credential)
        {
            var user = await _userRepo.GetUserByCredentialAsync(credential);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            SetCurrentUser(user);

            var accessToken = _tokenService.GenerateToken(user, TokenType.AccessToken);
            //var refreshToken=_tokenService.GenerateToken(user,TokenType.RefreshToken);

            return accessToken;
        }

        public Task<TokenDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission(UserRole requiredRole)
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("User must be authenticated to check permissions.");
            }

            var roles = new List<UserRole> { _currentUser.Role };

            if (_currentUser.Role == UserRole.SuperAdmin)
            {
                roles.AddRange(Enum.GetValues(typeof(UserRole)).Cast<UserRole>());
            }
            else if (_currentUser.Role == UserRole.Admin)
            {
                roles.Add(UserRole.User);
            }

            return roles.Contains(requiredRole);
        }
    }
}
