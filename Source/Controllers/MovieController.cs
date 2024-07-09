namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public sealed class MovieController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateMovieAsync(MovieCreationRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }
}