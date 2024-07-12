using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService) => _paymentService = paymentService;
    
    [HttpPost("checkout"), Authorize]
    public async Task<ActionResult<string>> CreateCheckoutSession()
        => Ok((await _paymentService.GenerateSession4Checkout()).Url);
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponder<bool>>> FulfillOrder()
    {
        ServiceResponder<bool> response = await _paymentService.CompleteOrder(Request);
        return response.IsSuccess ? Ok(response) : BadRequest(response.Message);
    }
}