using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Entity
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        public string Avatar { get; set; }

        [Required]
        [EmailAddress] //validation for email address that it need ot contain valid email fomat "example@example.com"
        [MaxLength(128)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public User(string firstName, string lastName, UserRole role, string avatar, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Avatar = avatar;
            Email = email;
            Password = password;
        }

        public User() { }
    }
}
