namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/identity")]
public sealed class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAccountAsync(AccountRegistrationRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }
}