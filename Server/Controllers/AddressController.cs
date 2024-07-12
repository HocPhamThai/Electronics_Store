using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AddressController : Controller
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService) => _addressService = addressService;

    [HttpGet]
    public async Task<ActionResult<ServiceResponder<Address>>> GetAddress()
        => await _addressService.GetAddress();
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponder<Address>>> AddOrUpdateAddress(Address address)
        => await _addressService.AddOrUpdateAddress(address);
}