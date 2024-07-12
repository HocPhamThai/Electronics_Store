using Electronics_Store.Shared.DTO;

namespace Electronics_Store.Client.Services.Service4Products_Client;

public class ProductsService: IProductsService
{
    private readonly HttpClient _httpClient;
    public event Action? ChangeOfProducts;

    public string Message { get; set; } = "Fetching Data, Please Wait!";
    public string LatestSearch { get; set; } = string.Empty;
    public int CurrentPageIndex { get; set; } = 1;
    public int NumOfPages { get; set; }

    public List<Product> Products { get; set; } = new();
    public List<Product> ProductsForAdmin { get; set; } = new();

    public ProductsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task GetProducts(string? categoryUrl)
    {
        if (!string.IsNullOrEmpty(categoryUrl))
        {
            ServiceResponder<List<Product>>? serviceResponder = await _httpClient.GetFromJsonAsync<ServiceResponder<List<Product>>>($"api/products/categories/{categoryUrl}");
            if (serviceResponder != null && serviceResponder.Data != null)
                Products = serviceResponder.Data;
            
            CurrentPageIndex = 1;
            NumOfPages = 0;

            if (Products.Count == 0)
                Message = "No products found";
            else
            {
                Message = $"List Of {categoryUrl} Products".ToUpper();
            }
            
            ChangeOfProducts.Invoke();
        }
        else
            await GetTopProducts();
    }

    public async Task GetProductsAsAdmin()
    {
        NumOfPages = 0;
        CurrentPageIndex = 1;
        ProductsForAdmin = (await   _httpClient
                                    .GetFromJsonAsync<ServiceResponder<List<Product>>>("api/Products/administrator")
                            )
                            .Data;
        if (ProductsForAdmin.Count == 0)
            Message = "There is 0 Product.";
    }

    private decimal ProductVariantPrice(Product product)
    {
        List<ProductVariant>? variants = product.ProductVariants;
        if (variants == null || !variants.Any())
            return 0;
        return variants.Min(variant => variant.price);
    }
    public void SortProductsByPrice(int num)
    {
        if (num >= 0)
            Products = Products.OrderBy(ProductVariantPrice).ToList();
        else
            Products = Products.OrderByDescending(ProductVariantPrice).ToList();
        ChangeOfProducts.Invoke();
    }
    
    public void SortProductsByAlphabet(int num)
    {
        if (num >= 0)
            Products = Products.OrderBy(product => product.ProductName).ToList();
        else
            Products =  Products.OrderByDescending(product => product.ProductName).ToList();
        ChangeOfProducts.Invoke();
    }

    public async Task GetProducts()
    {
        ServiceResponder<List<Product>>? serviceResponder = await _httpClient.GetFromJsonAsync<ServiceResponder<List<Product>>>("api/Products");
        if (serviceResponder != null && serviceResponder.Data != null)
            Products = serviceResponder.Data;
        ChangeOfProducts.Invoke();
    }

    public async Task GetTopProducts()
    {
        ServiceResponder<List<Product>>? responder = await _httpClient.GetFromJsonAsync<ServiceResponder<List<Product>>>("api/Products/top");
        if (responder != null && responder.Data != null)
            Products = responder.Data;
        ChangeOfProducts.Invoke();
    }

    public async Task<ServiceResponder<Product>> GetProductById(int id) => 
        await _httpClient.GetFromJsonAsync<ServiceResponder<Product>>($"api/Products/{id}");

    public async Task GetProductsBySearchText(string text, int currentPageIndex)
    {
        LatestSearch = text;
        ServiceResponder<ProductsDTO>? responder = await _httpClient.GetFromJsonAsync<ServiceResponder<ProductsDTO>>($"api/Products/search/{text}/{currentPageIndex}");
        if (responder is { Data: not null })
        {
            Products = responder.Data.products;
            CurrentPageIndex = responder.Data.currentPageIndex;
            NumOfPages = responder.Data.numOfPages;
            Message = (Products.Count == 0) ? "No Products Match Your Search" : Message;
        }
        ChangeOfProducts.Invoke();
    }

    public async Task<List<string>> GetSuggestionsBySearchText(string search) =>
        (await _httpClient.GetFromJsonAsync<ServiceResponder<List<string>>>($"api/Products/suggestions/{search}")).Data;

    
    //modifying features for administrator
    public async Task<Product> CreateProduct(Product product) =>
        (await (await   _httpClient
                        .PostAsJsonAsync("api/Products", product)
                ) /*result*/
                .Content
                .ReadFromJsonAsync<ServiceResponder<Product>>()
        ) /*response*/
        .Data/*new product*/;

    public async Task<Product> UpdateProduct(Product product) =>
        (await (await   _httpClient
                        .PutAsJsonAsync($"api/Products", product)
                ) /*result*/
                .Content
                .ReadFromJsonAsync<ServiceResponder<Product>>()
        ) /*response*/
        .Data;

    public async Task DeleteProduct(Product product) =>
        await _httpClient.DeleteAsync($"api/Products/{product.ProductId}");
    
}