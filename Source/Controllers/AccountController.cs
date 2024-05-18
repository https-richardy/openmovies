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
    public async Task<ActionResult> RegisterAsync(AccountRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = TinyMapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to create the user.", erros = result.Errors });

        if (user == null)
            return BadRequest(new { message = "Failed to create the user." });

        await _userManager.AddToRoleAsync(user, Role.CommonUser);

        var token = await _jwt.GenerateTokenResponseAsync(user);
        return StatusCode(201, token);
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return BadRequest(new { message = "Invalid credentials." });

        var token = await _jwt.GenerateTokenResponseAsync(user);
        return Ok(token);
    }
}