using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;

namespace Ecommerce.Service.src.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepo,
            ITokenService tokenService,
            IPasswordService passwordService,
            IMapper mapper
        )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _mapper=mapper;
        }

        public async Task<ResponseTokenReadDto> LoginAsync(UserCredential credential)
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
                var token=await _tokenService.GenerateToken(user);
                return _mapper.Map<ResponseTokenReadDto>(token);
            }
            else
            {
                throw new UnauthorizedAccessException("Password is not correct.");
            }
        }

        public async Task<ResponseTokenReadDto> RefreshTokenAsync(string refreshTokenDto)
        {
            var token= await _tokenService.RefreshToken(refreshTokenDto);
            return _mapper.Map<ResponseTokenReadDto>(token);
        }

        public async Task<bool> LogoutAsync(Guid userId)
        {
            return await _tokenService.InvalidateTokenAsync(userId);

        }

    }
}
