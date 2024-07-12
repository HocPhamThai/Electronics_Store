using System.Security.Claims;
using System.Text.Json;
using System.Net.Http.Headers;

using Blazored.LocalStorage;

namespace Electronics_Store.Client.Authentication;

public class CustomizedAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly ILocalStorageService _storageService;
    private readonly HttpClient _httpClient;

    public CustomizedAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _storageService = localStorage;
        _httpClient = httpClient;
    }
    
    private byte[] ParseBase64String(string base64String) // WithoutPadding
    {
        if (base64String.Length % 4 == 2)
            base64String += "==";
        else if (base64String.Length % 4 == 3)
            base64String += "=";
        return Convert.FromBase64String(base64String);
    }

    private IEnumerable<Claim> ParseClaimsGetFromJsonWebToken(string jwt) // FromJwt
        => JsonSerializer.Deserialize<Dictionary<string, object>>(
                ParseBase64String(
                                jwt.Split('.')[1] /*payload Part*/
                            )/*Json as bytes*/
            )? /*Key-value Pairs of input JSON*/
            .Select(pair => new Claim(pair.Key, pair.Value.ToString() ?? string.Empty));

    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string authenticationToken = await _storageService.GetItemAsStringAsync("Authentication_Token_Key");
        ClaimsIdentity? identity = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
            if (string.IsNullOrEmpty(authenticationToken)) {}
            else {   
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsGetFromJsonWebToken(authenticationToken), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken.Replace("\"", ""));
                }
                catch
                {
                    await _storageService.RemoveItemAsync("Authentication_Token_Key");
                    identity = new ClaimsIdentity();
                }
            }
            
            AuthenticationState authenticationState = new AuthenticationState(
                                                        new ClaimsPrincipal(identity??new ClaimsIdentity())/*the user*/
                                                    );
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        return authenticationState;
    }
}