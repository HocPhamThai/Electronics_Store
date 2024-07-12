using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService) => _productsService = productsService;
        

        [HttpGet]
        public async Task<ActionResult<ServiceResponder<List<Product>>>> GetProducts()
            => Ok(await _productsService.GetProductsAsync());
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponder<Product>>> GetProductById(int id)
            => Ok(await _productsService.GetProductByIdAsync(id));
        
        [HttpGet("suggestions/{search}")]
        public async Task<ActionResult<ServiceResponder<List<string>>>> GetSearchSuggestions(string search)
            => Ok(await _productsService.GetSearchSuggestionsAsync(search));
        
        [HttpGet("search/{text}/{currentPageIndex:int}")]
        public async Task<ActionResult<ServiceResponder<ProductsDTO>>> GetProductsIncludeText(string text, int currentPageIndex)
            => Ok(await _productsService.GetProductsIncludeText(text, currentPageIndex));

        [HttpGet("categories/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponder<List<Product>>>> GetProductsByCategory(string categoryUrl)
            => Ok(await _productsService.GetProductsByCategoryUrl(categoryUrl,0,"price"));
        
        [HttpGet("categories/{categoryUrl}/sort/{key}/{num}")]
        public async Task<ActionResult<ServiceResponder<List<Product>>>> SortProduct(string categoryUrl, int num, string key)
            => Ok(await _productsService.GetProductsByCategoryUrl(categoryUrl, num, key));
        
        [HttpGet("top")]
        public async Task<ActionResult<ServiceResponder<List<Product>>>> GetTopProducts()
            => Ok(await _productsService.GetTopProducts());
        
    
        
        [HttpGet("administrator"), Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ServiceResponder<List<Product>>>> GetAdminProducts()
            => Ok(await _productsService.GetProductsAsAdmin());

        [HttpPost, Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ServiceResponder<Product>>> CreateProduct(Product product)
            => Ok(await _productsService.CreateProduct(product));
        

        [HttpPut, Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ServiceResponder<Product>>> UpdateProduct(Product product)
            => Ok(await _productsService.UpdateProduct(product));

        [HttpDelete("{productId}"), Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ServiceResponder<bool>>> DeleteProductById(int productId)
            => Ok(await _productsService.DeleteProductById(productId));
    }
}
