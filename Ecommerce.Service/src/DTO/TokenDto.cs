
namespace Ecommerce.Service.src.DTO
{
    public class TokenDto
    {
        public class ResponseTokenReadDto{
            public string AccessToken{get;set;}
            public string RefreshToken{get;set;}
        }

        public class RefreshTokenDto{
            public string RefreshToken {get;set;}
        }
    }
}