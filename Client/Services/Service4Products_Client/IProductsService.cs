namespace Electronics_Store.Client.Services.Service4Products_Client;

public interface IProductsService
{
    event Action ChangeOfProducts;
    
    string Message { get; set; }
    string LatestSearch { get; set; }
    
    int CurrentPageIndex { get; set; }
    int NumOfPages { get; set; }
    
    
    List<Product> Products { get; set; }
    List<Product> ProductsForAdmin { get; set; }


    void SortProductsByPrice(int num);
    void SortProductsByAlphabet(int num);
    Task GetProducts();
    Task GetProducts(string? categoryUrl);
    Task GetProductsAsAdmin();
    
    Task<ServiceResponder<Product>> GetProductById(int id);
    
    Task GetProductsBySearchText(string searchText, int page);
    Task<List<string>> GetSuggestionsBySearchText(string searchText);
    
    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}