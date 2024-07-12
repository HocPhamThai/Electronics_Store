using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class ProductVarietyController : Controller
{
    private readonly IProductVarietyService _productVarietyService;

    public ProductVarietyController(IProductVarietyService productVarietyService)
        => _productVarietyService = productVarietyService;
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponder<List<ProductVariety>>>> AddProductType(ProductVariety productVariety)
        => Ok(await _productVarietyService.AddProductVariety(productVariety));
    
    [HttpPut]
    public async Task<ActionResult<ServiceResponder<List<ProductVariety>>>> UpdateProductType(ProductVariety productVariety)
        => Ok(await _productVarietyService.UpdateProductVariety(productVariety));
    
    [HttpGet]
    public async Task<ActionResult<ServiceResponder<List<ProductVariety>>>> GetProductVarieties()
        => Ok(await _productVarietyService.GetProductVarieties());
}