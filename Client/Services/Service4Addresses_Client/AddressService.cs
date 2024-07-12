namespace Electronics_Store.Client.Services.Service4Addresses_Client;

public class AddressService: IAddressService
{
    private readonly HttpClient _httpClient;

    public AddressService(HttpClient http) => _httpClient = http;

    public async Task<Address> GetAddress()
        => (await _httpClient.GetFromJsonAsync<ServiceResponder<Address>>("api/Address")).Data;

    public async Task<Address> AddOrUpdateAddress(Address address)
        => (await _httpClient.PostAsJsonAsync("api/Address", address))
            .Content.ReadFromJsonAsync<ServiceResponder<Address>>().Result.Data;
}