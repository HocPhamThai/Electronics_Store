namespace Electronics_Store.Server.Services.Service4Categories_Server;

public class CategoriesService : ICategoriesService
{
    private readonly ElectronicsStoreDbContext _dbContext;

    public CategoriesService(ElectronicsStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponder<List<Category>>> AddCategory(Category category)
    {
        category.isNew = false;
        category.isEditing = false;
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
        return await GetCategoriesAsAdmin();
    }

    public async Task<ServiceResponder<List<Category>>> DeleteCategoryById(int id)
    {
        Category? category = await GetCategoryById(id);
        if (category == null)
            return new ServiceResponder<List<Category>>
            {
                IsSuccess = false,
                Message = "Couldn't Find The Category with Specified Id!"
            };
        category.isDeleted = true;
        await _dbContext.SaveChangesAsync();
        return await GetCategoriesAsAdmin();
    }

    public async Task<ServiceResponder<List<Category>>> UpdateCategory(Category category)
    {
        Category? foundDbCategory = await GetCategoryById(category.CategoryId);
        if (foundDbCategory == null)
            return new ServiceResponder<List<Category>>
            {
                IsSuccess = false,
                Message = "Couldn't Find The Database Category Match The Category to Update!"
            };
        foundDbCategory.CategoryName = category.CategoryName;
        foundDbCategory.CategoryAccessUrl = category.CategoryAccessUrl;
        foundDbCategory.isViewable = category.isViewable;
        await _dbContext.SaveChangesAsync();
        return await GetCategoriesAsAdmin();
    }

    public async Task<ServiceResponder<List<Category>>> GetCategoriesAsync()
        => new()
        {
            Data = await _dbContext.Categories
                        .Where(c => !c.isDeleted && c.isViewable)
                        .ToListAsync()
        };







    private async Task<Category?> GetCategoryById(int categoryId)
        => await _dbContext.Categories.FirstOrDefaultAsync(category => category.CategoryId == categoryId);

    public async Task<ServiceResponder<List<Category>>> GetCategoriesAsAdmin()
        => new()
        {
            Data = await _dbContext.Categories
                        .Where(category => !category.isDeleted)
                        .ToListAsync()
        };
    
}