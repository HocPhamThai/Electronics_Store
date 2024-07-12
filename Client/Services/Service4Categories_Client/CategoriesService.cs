namespace Electronics_Store.Client.Services.Service4Categories_Client;

public class CategoriesService: ICategoriesService
{
    public List<Category> Categories { get; set; } = new();
    public List<Category> AdminCategories { get; set; } = new();
    public event Action? ChangeOfCategory;
    
    private readonly HttpClient _httpClient;
    
    public CategoriesService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task GetCategories()
    {
        ServiceResponder<List<Category>>? serviceResponder = await _httpClient.GetFromJsonAsync<ServiceResponder<List<Category>>>("api/Categories");
        if (serviceResponder != null && serviceResponder.Data != null) 
            Categories = serviceResponder.Data;
    }

    public async Task GetCategoriesAsAdmin()
    {
        ServiceResponder<List<Category>>? response = await _httpClient.GetFromJsonAsync<ServiceResponder<List<Category>>>("api/Categories/administrator");
        if (response is { Data: not null })
            AdminCategories = response.Data;
    }

    public Category CreateCategory()
    {
        Category category = new Category { isNew = true, isEditing = true };
        AdminCategories.Add(category);
        ChangeOfCategory.Invoke();
        return category;
    }

    public async Task AddCategory(Category category)
    {
        AdminCategories = (await (await _httpClient.PostAsJsonAsync("api/Categories/administrator", category))
                                .Content
                                .ReadFromJsonAsync<ServiceResponder<List<Category>>>())
                          .Data;
        await GetCategories();
        ChangeOfCategory.Invoke();
    }

    public async Task UpdateCategory(Category category)
    {
        AdminCategories = (await (await _httpClient.PutAsJsonAsync("api/Categories/administrator", category))
                                .Content
                                .ReadFromJsonAsync<ServiceResponder<List<Category>>>())
                          .Data;
        await GetCategories();
        ChangeOfCategory.Invoke();
    }

    public async Task DeleteCategoryById(int categoryId)
    {
        AdminCategories = (await (await _httpClient.DeleteAsync($"api/Categories/administrator/{categoryId}"))
                                .Content
                                .ReadFromJsonAsync<ServiceResponder<List<Category>>>())
                          .Data;
        await GetCategories();
        ChangeOfCategory.Invoke();
    }
}