using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Entity;

[Table("reviews")]
public class Review : BaseEntity
{
    public Review() { }

    [Required]
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User{get;set;}

    [Required]
    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; }

    public Product Product {get;set;}=null!;//reference

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
}
