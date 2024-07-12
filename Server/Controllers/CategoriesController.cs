
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : Controller
{
    private readonly ICategoriesService _categoriesService;
    public CategoriesController(ICategoriesService categoriesService) => _categoriesService = categoriesService;

    
    [HttpDelete("administrator/{categoryId}"), Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ServiceResponder<List<Category>>>> DeleteCategoryById(int categoryId)
        => Ok(await _categoriesService.DeleteCategoryById(categoryId));

    
    [HttpPost("administrator"), Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ServiceResponder<List<Category>>>> AddCategory(Category category)
        => Ok(await _categoriesService.AddCategory(category));

    
    [HttpPut("administrator"), Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ServiceResponder<List<Category>>>> UpdateCategory(Category category)
        => Ok(await _categoriesService.UpdateCategory(category));
    
    
    [HttpGet]
    public async Task<ActionResult<ServiceResponder<List<Category>>>> GetCategories()
        => Ok(await _categoriesService.GetCategoriesAsync());
    
    
    [HttpGet("administrator"), Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ServiceResponder<List<Category>>>> GetAdminCategories()
        => Ok(await _categoriesService.GetCategoriesAsAdmin());
}