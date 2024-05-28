namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IMediator _mediator;

    public MovieController(
        IMovieService movieService,
        IMediator mediator)
    {
        _movieService = movieService;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMoviesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var movies = await _movieService.GetAllMoviesAsync();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieByIdAsync(int id)
    {
        var retrievedMovie = await _movieService.GetMovieByIdAsync(id);
        return Ok(retrievedMovie);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovieAsync(MovieCreationRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode((int)HttpStatusCode.Created, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovieAsync(int id, UpdateMovieRequest request)
    {
        var existingMovie = await _movieService.GetMovieByIdAsync(id);
        await _movieService.UpdateMovieAsync(existingMovie);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieAsync(MovieDeletionRequest request)
    {
        var response = await _mediator.Send(request.MovieId);
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovieAsync(
        [FromQuery] string? name = null,
        [FromQuery] int? releaseYear = null,
        [FromQuery] int? categoryId = null)
    {
        var movies = await _movieService.SearchMoviesAsync(name, releaseYear, categoryId);
        return Ok(movies);
    }
}
