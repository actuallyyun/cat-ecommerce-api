
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Entity;

[Table("reviews")]
public class Review : BaseEntity
{
    public Review() { }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ProductId { get; set; }


    [Required]
    public bool IsAnonymous { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }
    private int _rating;

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating
    {
        get => _rating;
        set
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.");
            }
            _rating = value;
        }
    }
    public List<Image> Images { get; set; } = new List<Image>();

    [ForeignKey("ProductId")]
    public Product product { get; }

    [ForeignKey("UserId")]
    public User User { get; }

    public Review(Guid userId, Guid productId, bool isAnonymous, string content, int rating)
    {
        UserId = userId;
        ProductId = productId;
        IsAnonymous = isAnonymous;
        Content = content;
        Rating = rating;
    }
}
