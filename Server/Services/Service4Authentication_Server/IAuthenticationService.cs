namespace Electronics_Store.Server.Services.Service4Authentication_Server;

public interface IAuthenticationService
{
    int GetUserId();
    string GetUserEmail();
    Task<User?> GetUserByEmail(string email);
    Task<ServiceResponder<int>> SignUp(User user, string password);
    Task<bool> IsExistingUser(string email);
    Task<ServiceResponder<string>> SignIn(string enteredEmail, string enteredPassword);
    Task<ServiceResponder<bool>> ChangePassword(int userId, string oldPassword, string newPassword);
}