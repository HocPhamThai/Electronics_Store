namespace Electronics_Store.Client.Services.Service4Categories_Client;

public interface ICategoriesService
{
    List<Category> Categories { get; set; }
    List<Category> AdminCategories { get; set; }
    event Action ChangeOfCategory;
    
    Task GetCategories();
    Task GetCategoriesAsAdmin();
    Category CreateCategory();
    Task AddCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategoryById(int categoryId);
    
}