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
        public List<string> Images { get; set; }
    }

    public class ReviewUpdateDto
    {
        public bool? IsAnonymous { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public List<string> Images { get; set; }
    }

    public class ReviewReadDto:BaseEntity
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid ProductId { get; }
        public bool IsAnonymous { get; }
        public string Content { get; }
        public int Rating { get; }
        public List<Image> Images { get; }

        public ReviewReadDto(Guid id, Guid userId, Guid productId, bool isAnonymous, string content, int rating, List<Image> images)
        {
            Id = id;
            UserId = userId;
            ProductId = productId;
            IsAnonymous = isAnonymous;
            Content = content;
            Rating = rating;
            Images = images;
        }
    }
}
