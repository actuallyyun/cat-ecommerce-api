using Moq;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.Core.src.RepoAbstraction;

namespace Ecommerce.Tests.src.Service
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private AuthService _authService;

        private readonly User testSuperAdmin = new User(
            "superAdmin",
            "test",
            UserRole.SuperAdmin,
            "avatarlink",
            "superadmin@mail.com",
            "secure"
        );

        private readonly User testAdmin = new User(
            "admin",
            "test",
            UserRole.Admin,
            "avatarlink",
            "admin@mail.com",
            "secure"
        );

        private readonly User testUser = new User(
            "user",
            "test",
            UserRole.User,
            "avatarlink",
            "user@mail.com",
            "secure"
        );

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(_mockUserRepo.Object, _mockTokenService.Object);
        }

        [Theory]
        [InlineData("superadmin@mail.com", "secure")]
        public async Task Login_WithSuperAdminRole_ShouldHaveAllPermissions(string email, string password)
        {
            var userCredential = new UserCredential(email, password);

            _mockUserRepo.Setup(repo => repo.GetUserByCredentialAsync(userCredential))
                .ReturnsAsync(testSuperAdmin);

            _mockTokenService.Setup(service => service.GenerateToken(testSuperAdmin, TokenType.AccessToken))
                .Returns("SuperAdminToken");

            await _authService.LoginAsync(userCredential);

            Assert.True(_authService.HasPermission(UserRole.SuperAdmin));
            Assert.True(_authService.HasPermission(UserRole.Admin));
            Assert.True(_authService.HasPermission(UserRole.User));
        }

        [Fact]
        public async Task Login_WithAdminRole_ShouldHaveAdminPermissions()
        {
            var userCredential = new UserCredential("admin@mail.com", "secure");

            _mockUserRepo.Setup(repo => repo.GetUserByCredentialAsync(userCredential))
                .ReturnsAsync(testAdmin);

            _mockTokenService.Setup(service => service.GenerateToken(testAdmin, TokenType.AccessToken))
                .Returns("AdminToken");

            await _authService.LoginAsync(userCredential);

            Assert.False(_authService.HasPermission(UserRole.SuperAdmin));
            Assert.True(_authService.HasPermission(UserRole.Admin));
            Assert.True(_authService.HasPermission(UserRole.User));
        }

        [Fact]
        public async Task Login_WithUserRole_ShouldOnlyHaveUserPermission()
        {
            var userCredential = new UserCredential("user@mail.com", "secure");

            _mockUserRepo.Setup(repo => repo.GetUserByCredentialAsync(userCredential))
                .ReturnsAsync(testUser);

            _mockTokenService.Setup(service => service.GenerateToken(testUser, TokenType.AccessToken))
                .Returns("UserToken");

            await _authService.LoginAsync(userCredential);

            Assert.False(_authService.HasPermission(UserRole.SuperAdmin));
            Assert.False(_authService.HasPermission(UserRole.Admin));
            Assert.True(_authService.HasPermission(UserRole.User));
        }
    }
}
