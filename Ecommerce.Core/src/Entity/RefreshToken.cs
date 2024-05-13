namespace Ecommerce.Core.src.Entity
{
    public class RefreshToken:BaseEntity
    {
        public Guid UserId {get;}
        public DateTime ExpiresAt {get;set;}
        public bool IsRevoked {get;set;}
    }
}