namespace Ecommerce.Service.src.DTO
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TokenDto(
            string accessToken,
            string refreshToken
        )
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }
}
