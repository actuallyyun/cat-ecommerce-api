using System.Text.RegularExpressions;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.Validation
{
    public static class UserValidation
    {
        private static readonly Regex EmailRegex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);

        public static void ValidateUserCreateDto(UserCreateDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.FirstName) || string.IsNullOrWhiteSpace(userDto.LastName))
            {
                throw new ArgumentException("First name and last name must be provided.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Email) || !IsValidEmail(userDto.Email))
            {
                throw new ArgumentException("A valid email must be provided.");
            }

            if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }
        }

        public static void ValidateUserUpdateDto(UserUpdateDto userDto)
        {
            if (userDto.Password != null && userDto.Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }
        }

        private static bool IsValidEmail(string email)
        {
            return EmailRegex.IsMatch(email);
        }
    }
}
