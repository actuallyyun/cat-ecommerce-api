namespace Ecommerce.Core.src.Entity
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }

    public class ResponseToken : BaseEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
