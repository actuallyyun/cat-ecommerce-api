using Ecommerce.Core.src.Entity;
using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Tests.src.Core
{
    public class UserTests
    {
        // Helper method to validate a User object
        private void ValidateUser(User user)
        {
            var validationContext = new ValidationContext(user);
            Validator.ValidateObject(user, validationContext, validateAllProperties: true);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var firstName = "John";
            var lastName = "Doe";
            var role = UserRole.User;
            var avatar = "http://example.com/avatar.jpg";
            var email = "john.doe@example.com";
            var password = "securePassword123";

            var user = new User(firstName, lastName, role, avatar, email, password);

            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(role, user.Role);
            Assert.Equal(avatar, user.Avatar);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
        }

        [Fact]
        public void Constructor_WithEmptyFirstName_ShouldThrowValidationException()
        {
            var lastName = "Doe";
            var role = UserRole.User;
            var avatar = "http://example.com/avatar.jpg";
            var email = "john.doe@example.com";
            var password = "securePassword123";

            var user = new User("", lastName, role, avatar, email, password);

            Assert.Throws<ValidationException>(() => ValidateUser(user));
        }

        [Fact]
        public void Constructor_WithInvalidEmail_ShouldThrowValidationException()
        {
            var firstName = "John";
            var lastName = "Doe";
            var role = UserRole.User;
            var avatar = "http://example.com/avatar.jpg";
            var email = "invalid-email";
            var password = "securePassword123";

            var user = new User(firstName, lastName, role, avatar, email, password);

            Assert.Throws<ValidationException>(() => ValidateUser(user));
        }


        [Fact]
        public void UpdatePassword_ShouldChangePasswordSuccessfully()
        {
            var firstName = "John";
            var lastName = "Doe";
            var role = UserRole.User;
            var avatar = "http://example.com/avatar.jpg";
            var email = "john.doe@example.com";
            var initialPassword = "securePassword123";

            var user = new User(firstName, lastName, role, avatar, email, initialPassword);

            // Update the password
            var newPassword = "newSecurePassword456";
            user.Password = newPassword;

            Assert.Equal(newPassword, user.Password);
        }

        [Fact]
        public void Constructor_WithNullAvatar_ShouldInitializeToDefault()
        {
            var firstName = "John";
            var lastName = "Doe";
            var role = UserRole.User;
            var email = "john.doe@example.com";
            var password = "securePassword123";

            var user = new User(firstName, lastName, role, null, email, password);

            // Default avatar is represented here by a null check
            Assert.Null(user.Avatar);
        }
    }
}
