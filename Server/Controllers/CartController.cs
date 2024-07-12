using Microsoft.AspNetCore.Mvc;



namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
        => _cartService = cartService;
    
    [HttpPost("products")]
    public async Task<ActionResult<ServiceResponder<List<CartProductResponder>>>> GetCartProducts(List<CartProduct> cartProducts)
        => Ok(await _cartService.GetCartProductResponses(cartProducts));
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponder<List<CartProductResponder>>>> PersistCartProducts(List<CartProduct> cartProducts)
        => Ok(await _cartService.PersistCartProducts(cartProducts));
    
    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponder<bool>>> Add2Cart(CartProduct cartProduct)
        => Ok(await _cartService.Add2Cart(cartProduct));
    

    [HttpPut("update-quantity")]
    public async Task<ActionResult<ServiceResponder<bool>>> UpdateQuantity(CartProduct cartProduct)
        => Ok(await _cartService.UpdateQuantity(cartProduct));
    

    [HttpDelete("{productId}/{productVarietyId}")]
    public async Task<ActionResult<ServiceResponder<bool>>> RemoveCartProduct(int productId, int productVarietyId)
        => Ok(await _cartService.RemoveCartProduct(productId, productVarietyId));

    [HttpGet("count")]
    public async Task<ActionResult<ServiceResponder<int>>> CountCartProducts()
        => await _cartService.CountCartProducts();
    

    [HttpGet]
    public async Task<ActionResult<ServiceResponder<List<CartProductResponder>>>> GetUserCartProducts()
        => Ok(await _cartService.GetUserCartProducts());
    
}