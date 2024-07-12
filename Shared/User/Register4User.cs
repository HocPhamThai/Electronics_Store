using System.ComponentModel.DataAnnotations;

namespace Electronics_Store.Shared.User;

public class Register4User
{
    [Required, EmailAddress]
    public string email { get; set; } = string.Empty;
    
    [Required, StringLength(100, MinimumLength = 8)]
    public string password { get; set; } = string.Empty;
    [Compare("password", ErrorMessage = "Passwords Unmatch! Please Check Your Passwords Again")]
    public string userConfirmedPassword { get; set; } = string.Empty;
}