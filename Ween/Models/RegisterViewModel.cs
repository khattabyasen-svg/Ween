using System.ComponentModel.DataAnnotations;

namespace Ween.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    [Display(Name = "Phone")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}
