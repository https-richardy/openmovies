namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddNewProfileAsync(AddNewProfileRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }
}