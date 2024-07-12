using Electronics_Store.Shared.User;

namespace Electronics_Store.Client.Services.Service4Authentication_Client;

public interface IAuthenticationService
{
    Task<bool> IsAuthenticatedUser();
    Task<ServiceResponder<string>> Login(Login4User request2LogIn);
    Task<ServiceResponder<int>> Register(Register4User request2Register);
    Task<ServiceResponder<bool>> ChangePassword(PasswordChange4User request2ChangePassword);
}