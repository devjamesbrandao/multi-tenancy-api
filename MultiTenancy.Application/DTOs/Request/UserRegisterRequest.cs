using System.ComponentModel.DataAnnotations;

namespace MultiTenancy.Application.DTOs.Request;

public class UserRegisterRequest
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} is invalid")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(50, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 6)]
    public string Password { get; set; }
    
    [Compare(nameof(Password), ErrorMessage = "Passwords must be the same")]
    public string PasswordConfirmation { get; set; }
}