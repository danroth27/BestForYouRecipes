using System.ComponentModel.DataAnnotations;

namespace BestForYouRecipes.Data;

public class User
{
    public string Id { get; set; } = default!;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = default!;
    
    [Required]
    [MinLength(6)]
    public string PasswordHash { get; set; } = default!;
    
    public string? FullName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

