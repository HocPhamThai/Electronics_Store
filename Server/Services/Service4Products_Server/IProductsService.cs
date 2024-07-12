namespace Electronics_Store.Server.Services.Service4Products_Server;

public interface IProductsService
{
    Task<ServiceResponder<List<Product>>> GetProductsAsync();
    Task<ServiceResponder<List<Product>>> GetTopProducts();
    Task<ServiceResponder<List<Product>>> GetProductsAsAdmin();
    
    Task<ServiceResponder<Product>> GetProductByIdAsync(int productId);
    Task<ServiceResponder<ProductsDTO>> GetProductsIncludeText(string text, int page);
    
    Task<ServiceResponder<List<string>>> GetSearchSuggestionsAsync(string search);

    Task<ServiceResponder<List<Product>>> GetProductsByCategoryUrl(string categoryUrl, int num, string key);
    
    
    Task<ServiceResponder<Product>> UpdateProduct(Product product);
    Task<ServiceResponder<bool>> DeleteProductById(int productId);
    Task<ServiceResponder<Product>> CreateProduct(Product product);
}