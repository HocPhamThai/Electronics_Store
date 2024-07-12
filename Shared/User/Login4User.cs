using System.ComponentModel.DataAnnotations;

namespace Electronics_Store.Shared.User;

public class Login4User
{
    [Required]
    public string enteredEmail { get; set; } = string.Empty;
    [Required]
    public string enteredPassword { get; set; } = string.Empty;
}