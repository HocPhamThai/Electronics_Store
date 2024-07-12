namespace Electronics_Store.Client.Services.Service4Order_Client;

public class OrderService: IOrderService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    public OrderService(HttpClient http, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
    }

    private async Task<bool> IsAuthenticatedUser() => (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated??false;


    public async Task<string>  PutOrder()
    => await IsAuthenticatedUser() ?
            await (await _http.PostAsync("api/payment/checkout", null))/*result*/
                    .Content.ReadAsStringAsync()/*url string*/
        :   "login";

    public async Task<List<SummaryOrderResponder>> GetOrders() =>
        (await _http.GetFromJsonAsync<ServiceResponder<List<SummaryOrderResponder>>>("api/order")).Data;

    public async Task<DetailOrderResponder> GetDetail(int orderId) =>
        (await _http.GetFromJsonAsync<ServiceResponder<DetailOrderResponder>>($"api/order/{orderId}")).Data;
}