using FluentValidation;
using OpenMovies.DTOs;
using OpenMovies.Services;
using OpenMovies.Utils;

namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _hostEnvironment;


    public MovieController(
        IMovieService movieService,
        ICategoryService categoryService,
        IWebHostEnvironment hostEnvironment)
    {
        _movieService = movieService;
        _categoryService = categoryService;
        _hostEnvironment = hostEnvironment;
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm, FromBody] MovieDTO data)
    {
        try
        {
            var category = await _categoryService.GetCategoryById(data.CategoryId);

            var movie = new Movie(data.Title, data.ReleaseDateOf, data.Synopsis, category);

            if (data.Cover != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(data.Cover.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await data.Cover.CopyToAsync(fileStream);
                }

                movie.CoverImagePath = Path.Combine("images", fileName);
            }

            await _movieService.CreateMovie(movie);

            return StatusCode(201, movie);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] MovieDTO data)
    {
        try
        {
            var existingMovie = await _movieService.GetMovieById(id);

            existingMovie.Title = data.Title;
            existingMovie.ReleaseDateOf = data.ReleaseDateOf;
            existingMovie.Synopsis = data.Synopsis;

            var category = await _categoryService.GetCategoryById(data.CategoryId);

            existingMovie.Category = category;

            if (data.Cover != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(data.Cover.FileName);
                string path = Path.Combine(wwwRootPath, "images", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await data.Cover.CopyToAsync(fileStream);
                }

                existingMovie.CoverImagePath = Path.Combine("images", fileName);
            }

            await _movieService.UpdateMovie(existingMovie);

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors });
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
