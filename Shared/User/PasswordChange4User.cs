namespace Electronics_Store.Shared.User;

using System.ComponentModel.DataAnnotations;

public class PasswordChange4User
{
    [Required, StringLength(100, MinimumLength = 8)]
    public string UserOldPassword { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 8)]
    public string UserNewPassword { get; set; } = string.Empty;
    [Compare("UserNewPassword", ErrorMessage = "Passwords Unmatch! Please Check Your Passwords Again")]
    public string UserConfirmedPassword { get; set; } = string.Empty;
}