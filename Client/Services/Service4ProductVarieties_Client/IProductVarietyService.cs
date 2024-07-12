namespace Electronics_Store.Client.Services.Service4ProductVarieties_Client;

public interface IProductVarietyService
{
    public List<ProductVariety> ProductVarieties { get; set; }
    event Action ChangeOfProductVariety;
    
    Task GetProductVarieties();
    ProductVariety CreateProductVariety();
    Task AddProductVariety(ProductVariety productVariety);
    Task UpdateProductVariety(ProductVariety productVariety);
    
}