namespace Electronics_Store.Server.Services.Service4ProductVarieties_Server;

public interface IProductVarietyService
{
    Task<ServiceResponder<List<ProductVariety>>> AddProductVariety(ProductVariety productType);
    Task<ServiceResponder<List<ProductVariety>>> UpdateProductVariety(ProductVariety productType);
    
    Task<ServiceResponder<List<ProductVariety>>> GetProductVarieties();
}