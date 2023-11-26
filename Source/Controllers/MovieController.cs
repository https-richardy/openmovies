using Microsoft.AspNetCore.Mvc;
using OpenMovies.DTOs;
using OpenMovies.Models;
using OpenMovies.Services;

namespace OpenMovies.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly CategoryService _categoryService;
    private readonly DirectorService _directorService;


    public MovieController(
        MovieService movieService,
        CategoryService categoryService,
        DirectorService directorService)
    {
        _movieService = movieService;
        _categoryService = categoryService;
        _directorService = directorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieDTO data)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var director = await _directorService.GetDirectorById(data.DirectorId);
            var category = await _categoryService.GetCategoryById(data.CategoryId);

            var movie = new Movie(data.Title, data.ReleaseDateOf, data.Synopsis, director, category);

            if (data.Trailers != null && data.Trailers.Any())
            {
                var trailers = _movieService.CreateTrailers(data.Trailers, movie);
                await _movieService.AddTrailersToMovie(movie, trailers);
            }

            await _movieService.CreateMovie(movie);

            return StatusCode(201, movie);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
