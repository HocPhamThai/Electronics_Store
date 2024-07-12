namespace Electronics_Store.Client.Services.Service4ProductVarieties_Client;

public class ProductVarietyService: IProductVarietyService
{
    public List<ProductVariety> ProductVarieties { get; set; } =  new();
    public event Action? ChangeOfProductVariety;
    
    private readonly HttpClient _http;

    public ProductVarietyService(HttpClient http) => _http = http;
    
    public async Task GetProductVarieties()
        => ProductVarieties = (await _http.GetFromJsonAsync<ServiceResponder<List<ProductVariety>>>("api/ProductVariety")).Data;

    public ProductVariety CreateProductVariety()
    {
        ProductVariety productVariety = new(){isNew = true, isModifying = true};
        ProductVarieties.Add(productVariety);
        ChangeOfProductVariety.Invoke();
        return productVariety;
    }

    public async Task AddProductVariety(ProductVariety productVariety)
    {
        ProductVarieties = (await (await _http.PostAsJsonAsync("api/ProductVariety", productVariety))
                                    .Content
                                    .ReadFromJsonAsync<ServiceResponder<List<ProductVariety>>>())
                            .Data;
        ChangeOfProductVariety.Invoke();
    }

    public async Task UpdateProductVariety(ProductVariety productVariety)
    {
        ProductVarieties = (await (await _http.PutAsJsonAsync("api/ProductVariety", productVariety))
                                    .Content
                                    .ReadFromJsonAsync<ServiceResponder<List<ProductVariety>>>())
                            .Data;
        ChangeOfProductVariety.Invoke();
    }
}