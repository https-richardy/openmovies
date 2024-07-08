namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(AccountRegistrationRequest request)
    {
            var response = await _mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created, response);
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}