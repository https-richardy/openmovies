using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OpenMovies.DTOs;

namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuthService _jwt;

    public AccountController(UserManager<IdentityUser> userManager, AuthService jwt)
    {
        _userManager = userManager;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await CreateUserAsync(request);

        if (user == null)
            return BadRequest(new { message = "Failed to create the user." });

        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role,"Common"));

        var token = await _jwt.GenerateTokenResponseAsync(user);
        return StatusCode(201, token);
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> Authenticate(AuthenticateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return BadRequest(new { message = "Invalid credentials." });

        var token = await _jwt.GenerateTokenResponseAsync(user);
        return Ok(token);
    }

    private async Task<IdentityUser?> CreateUserAsync(RegisterRequest request)
    {
        var user = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        return result.Succeeded ? user : null;
    }
}