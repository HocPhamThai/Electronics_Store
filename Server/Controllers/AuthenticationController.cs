using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Electronics_Store.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthenticationController(IAuthenticationService authService) => _authenticationService = authService;
    
    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponder<int>>> Register(Register4User register)
    {
        ServiceResponder<int> response = await _authenticationService.SignUp(new User{userEmail = register.email}, register.password);
        return (response.IsSuccess) ? Ok(response) : BadRequest(response);
    }
    
    [HttpPost("signin")]
    public async Task<ActionResult<ServiceResponder<string>>> Login(Login4User request)
    {
        ServiceResponder<string> response = await _authenticationService.SignIn(request.enteredEmail, request.enteredPassword);
        return (response.IsSuccess) ? Ok(response) : BadRequest(response);
    }

    [HttpPost("changePassword"), Authorize]
    public async Task<ActionResult<ServiceResponder<bool>>> ChangePassword([FromBody] string[] passwords)
    {
        ServiceResponder<bool> response = await _authenticationService.ChangePassword(
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)??string.Empty)/*userId*/
            , passwords[0], passwords[1]
        );
        
        return (response.IsSuccess) ? Ok(response) : BadRequest(response);
    }
}