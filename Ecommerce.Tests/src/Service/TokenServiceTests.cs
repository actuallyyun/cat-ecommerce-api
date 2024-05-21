using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.WebApi.src.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Ecommerce.Tests.src.Service
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IRefreshTokenRepo> _mockRefreshTokenRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;

        public TokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockRefreshTokenRepo = new Mock<IRefreshTokenRepo>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockConfiguration.Setup(x => x["Secrets:JwtKey"]).Returns("superlongsecretotherwiseitcomplaints");
            _mockConfiguration.Setup(x => x["Secrets:Issuer"]).Returns("issuer");
        }

        [Fact]
        public async void GenerateToken_Should_GenerateAndReturnToken()
        {
            var tokenService = new TokenService(
                _mockConfiguration.Object,
                _mockRefreshTokenRepo.Object,
                _mockUserRepo.Object
            );
            var user = new User
            {
                Email = "user@mail.com",
                Id = Guid.NewGuid(),
                Role = UserRole.User,
                Salt = []
            };

            _mockRefreshTokenRepo
                .Setup(x => x.CreateAsync(It.IsAny<RefreshToken>()))
                .Returns(It.IsAny<Task<RefreshToken>>());

            var token = await tokenService.GenerateToken(user);

            Assert.IsType<ResponseToken>(token);
            _mockRefreshTokenRepo.Verify(x => x.CreateAsync(It.IsAny<RefreshToken>()),Times.Once());
        }

        [Fact]
        public async void RefreshToken_With_Valid_Token_ShouldRefreshAndReturnNewToken()
        {
            var tokenService = new TokenService(
                _mockConfiguration.Object,
                _mockRefreshTokenRepo.Object,
                _mockUserRepo.Object
            );
            var user = new User
            {
                Email = "user@mail.com",
                Id = Guid.NewGuid(),
                Role = UserRole.User,
                Salt = []
            };
            var refreshToken = "token";
            var newRefreshToken = "new token";
            var resfreshTokenInStorage = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
            };

            _mockConfiguration.Setup(x => x["Secrets:JwtKey"]).Returns("secret");
            _mockConfiguration.Setup(x => x["Secrets:Issuer"]).Returns("issuer");
            _mockRefreshTokenRepo
                .Setup(x => x.CreateAsync(It.IsAny<RefreshToken>()))
                .Returns(It.IsAny<Task<RefreshToken>>());
            _mockRefreshTokenRepo
                .Setup(x => x.GetAsync(refreshToken))
                .ReturnsAsync(resfreshTokenInStorage);
            _mockUserRepo
                .Setup(x => x.GetUserByIdAsync(resfreshTokenInStorage.UserId))
                .ReturnsAsync(user);

            _mockRefreshTokenRepo
                .Setup(x => x.UpdateAsync(user.Id, newRefreshToken))
                .ReturnsAsync(true);

            var token =await  tokenService.GenerateToken(user);

            Assert.IsType<ResponseToken>(token);
            _mockRefreshTokenRepo.Verify(x=>x.UpdateAsync(user.Id, newRefreshToken),Times.Once());

        }

        [Fact]
        public void RefreshToken_With_InValid_Token_ShouldThrowError()
        {
            var tokenService = new TokenService(
                _mockConfiguration.Object,
                _mockRefreshTokenRepo.Object,
                _mockUserRepo.Object
            );
            var user = new User
            {
                Email = "user@mail.com",
                Id = Guid.NewGuid(),
                Role = UserRole.User,
                Salt = []
            };
            var refreshToken = "token";
            var newRefreshToken = "new token";
            var resfreshTokenInStorage = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
            };

            _mockRefreshTokenRepo
                .Setup(x => x.CreateAsync(It.IsAny<RefreshToken>()))
                .ReturnsAsync(It.IsAny<RefreshToken>());
            
            RefreshToken token1=null;
            _mockRefreshTokenRepo.Setup(x => x.GetAsync(refreshToken)).ReturnsAsync(token1);

            _mockUserRepo
                .Setup(x => x.GetUserByIdAsync(resfreshTokenInStorage.UserId))
                .ReturnsAsync(user);

            _mockRefreshTokenRepo
                .Setup(x => x.UpdateAsync(user.Id, newRefreshToken))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<SecurityTokenException>(()=>tokenService.GenerateToken(user));
        }
    }
}
