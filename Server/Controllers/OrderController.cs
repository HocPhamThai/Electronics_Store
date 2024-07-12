using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService) => _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<ServiceResponder<List<SummaryOrderResponder>>>> GetOrders() => Ok(await _orderService.GetOrders());

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ServiceResponder<DetailOrderResponder>>> GetDetails(int orderId) => Ok(await _orderService.GetDetails(orderId));
}