namespace Electronics_Store.Server.Services.Service4ProductVarieties_Server;

public class ProductVarietyService: IProductVarietyService
{
    private readonly ElectronicsStoreDbContext _dbContext;

    public ProductVarietyService(ElectronicsStoreDbContext context) => _dbContext = context;
    
    public async Task<ServiceResponder<List<ProductVariety>>> AddProductVariety(ProductVariety productType)
    {
        productType.isNew = false;
        productType.isModifying = false;
        _dbContext.ProductVarieties.Add(productType);
        await _dbContext.SaveChangesAsync();
        return await GetProductVarieties();
    }

    public async Task<ServiceResponder<List<ProductVariety>>> UpdateProductVariety(ProductVariety productType)
    {
        ProductVariety? dbProductType = await _dbContext.ProductVarieties.FindAsync(productType.Id);
        if (dbProductType != null)
        {
            dbProductType.Name = productType.Name;
            await _dbContext.SaveChangesAsync();
            return await GetProductVarieties();
        }
        return new ServiceResponder<List<ProductVariety>> { IsSuccess = false, Message = "Couldn't find the Variety!" };
    }

    public async Task<ServiceResponder<List<ProductVariety>>> GetProductVarieties()
        => new() { Data = await _dbContext.ProductVarieties.ToListAsync() };
}