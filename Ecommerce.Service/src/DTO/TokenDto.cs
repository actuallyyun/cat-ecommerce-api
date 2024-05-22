namespace Ecommerce.Service.src.DTO
{
  
        public class ResponseTokenReadDto{
            public string AccessToken{get;}
            public string RefreshToken{get;}
        }

        public class RefreshTokenDto{
            public string RefreshToken {get;set;}
        }
    
}