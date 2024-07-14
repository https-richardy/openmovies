namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public sealed class MovieController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMoviesAsync([FromQuery] MovieRetrievalRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetMovieAsync(int movieId)
    {
        var response = await mediator.Send(new MovieDetailsRequest
        {
            MovieId = movieId
        });

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateMovieAsync(MovieCreationRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{movieId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateMovieAsync(MovieUpdateRequest request, int movieId)
    {
        request.MovieId = movieId;

        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{movieId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteMovieAsync(int movieId)
    {
        var response = await mediator.Send(new MovieDeletionRequest
        {
            MovieId = movieId
        });

        return StatusCode(response.StatusCode, response);
    }
}