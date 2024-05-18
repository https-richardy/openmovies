namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ICategoryService _categoryService;
    private readonly IFileUploadService _fileUploadService;

    public MovieController(
        IMovieService movieService,
        ICategoryService categoryService,
        IFileUploadService fileUploadService)
    {
        _movieService = movieService;
        _categoryService = categoryService;
        _fileUploadService = fileUploadService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var movies = await _movieService.GetAllMovies();
            var pagination = new Pagination<Movie>(movies, pageNumber, pageSize, HttpContext);

            return Ok(pagination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var retrievedMovie = await _movieService.GetMovieById(id);
            return Ok(retrievedMovie);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(MovieCreationRequest request)
    {
        try
        {
            var category = await _categoryService.GetCategoryById(request.CategoryId);

            var movie = TinyMapper.Map<Movie>(request);

            if (request.Cover != null)
            {
                var imagePath = await _fileUploadService.UploadFileAsync(request.Cover);
                movie.ImagePath = imagePath;
            }

            await _movieService.CreateMovie(movie);

            return StatusCode(201, movie);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMovieRequest request)
    {
        try
        {
            var existingMovie = await _movieService.GetMovieById(id);
            existingMovie = TinyMapper.Map<Movie>(request);

            var category = await _categoryService.GetCategoryById(request.CategoryId);

            existingMovie.Category = category;

            if (request.Cover != null)
            {
                var imagePath = await _fileUploadService.UploadFileAsync(request.Cover);
                existingMovie.ImagePath = imagePath;
            }

            await _movieService.UpdateMovie(existingMovie);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _movieService.DeleteMovie(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name = null,
        [FromQuery] int? releaseYear = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var movies = await _movieService.SearchMovies(name, releaseYear, categoryId);
            var pagination = new Pagination<Movie>(movies, pageNumber, pageSize, HttpContext);

            return Ok(pagination);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
