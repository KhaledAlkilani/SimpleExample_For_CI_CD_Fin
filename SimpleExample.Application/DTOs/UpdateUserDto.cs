using System.ComponentModel.DataAnnotations;

namespace SimpleExample.Application.DTOs;

public class UpdateUserDto
{
    [Required(ErrorMessage = "Firstname is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Firstname must be 3-100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name must be 3-100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be valid.")]
    [StringLength(255, ErrorMessage = "Email can be at most 255 characters.")]
    public string Email { get; set; } = string.Empty;
}
