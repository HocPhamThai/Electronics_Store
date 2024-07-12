namespace Electronics_Store.Server.Services.Service4Categories_Server;

public interface ICategoriesService
{
    Task<ServiceResponder<List<Category>>> AddCategory(Category category);
    Task<ServiceResponder<List<Category>>> DeleteCategoryById(int id);
    Task<ServiceResponder<List<Category>>> UpdateCategory(Category category);
    Task<ServiceResponder<List<Category>>> GetCategoriesAsync();
    Task<ServiceResponder<List<Category>>> GetCategoriesAsAdmin();
}