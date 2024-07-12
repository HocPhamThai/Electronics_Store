namespace Electronics_Store.Client.Services.Service4Authentication_Client;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _stateProvider;

    public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider stateProvider)
    {
        _httpClient = httpClient;
        _stateProvider = stateProvider;
    }
    
    public async Task<bool> IsAuthenticatedUser() => (await _stateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated??false;

    public async Task<ServiceResponder<int>> Register(Register4User request2Register)
        => await (await _httpClient.PostAsJsonAsync("api/Authentication/register",
                   request2Register)) /*response message*/
               .Content
               .ReadFromJsonAsync<ServiceResponder<int>>() ??
           new ServiceResponder<int>()
           {
               IsSuccess = false, Message = "Can't Sign You Up, Please Try Again Later"
           }; // get user id if sign up successfully
    
    public async Task<ServiceResponder<string>> Login(Login4User request2LogIn)
        => await (await _httpClient.PostAsJsonAsync("api/authentication/signin", request2LogIn)) /*response message*/
               .Content
               .ReadFromJsonAsync<ServiceResponder<string>>() ??
           new ServiceResponder<string>()
           {
               IsSuccess = false, Message = "Can't Log You In, Please Try Again Later"
           }; /*get login message or jwt if login successfully*/

    public async Task<ServiceResponder<bool>> ChangePassword(PasswordChange4User request2ChangePassword) =>
    await  (await _httpClient.PostAsJsonAsync(
             "api/authentication/changePassword",
                new string[]
                    {
                        request2ChangePassword.UserOldPassword, request2ChangePassword.UserNewPassword
                    })) /*Http response result as a message*/
            .Content
            .ReadFromJsonAsync<ServiceResponder<bool>>() ??
            new ServiceResponder<bool>(){ IsSuccess = false, Message = "Can't Change Password, Please Try Again Later" };
    
}