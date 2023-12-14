using System.ComponentModel.DataAnnotations;

namespace StarRatings;

public class Review
{
    [Required]
    [Range(1, 5)]
    public double Rating { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Text must be no more than 50 characters.")]
    public string? Text { get; set; }
}
