using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class UserCreateDto
    {
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public ImageCreateDto Avatar { get; set;}
        public string Email { get; set;}
        public string Password { get; set;}
    }

    public class UserReadDto
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRole Role { get; }
        public string Avatar { get; }
        public string Email { get; }

        public UserReadDto(
            Guid id,
            string firstName,
            string lastName,
            UserRole role,
            string avatar,
            string email
        )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Avatar = avatar;
            Email = email;
        }
    }

    public class UserUpdateDto
    {
        public string? FirstName { get; }
        public string? LastName { get; }
        public Image? Avatar { get; }
        public string? Password { get; }

    }
}
