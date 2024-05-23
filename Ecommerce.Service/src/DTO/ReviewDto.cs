using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ReviewCreateDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public bool IsAnonymous { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }

    public class ReviewUpdateDto
    {
        public bool? IsAnonymous { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
    }

    public class ReviewReadDto:BaseEntity
    {
        public Guid Id { get; set;}
        public UserReadDto User{ get; set;}
        public Guid ProductId { get;set; }
        public bool IsAnonymous { get; set;}
        public string Content { get; set;}
        public int Rating { get;set; }
    }
}
