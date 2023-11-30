using FluentValidation;
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
    public async Task<IActionResult> Create(MovieDTO data)
    {
        try
        {
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
    public async Task<IActionResult> Update(int id, MovieDTO data)
    {
        try
        {
            var existingMovie = await _movieService.GetMovieById(id);

            existingMovie.Title = data.Title;
            existingMovie.ReleaseDateOf = data.ReleaseDateOf;
            existingMovie.Synopsis = data.Synopsis;

            var director = await _directorService.GetDirectorById(data.DirectorId);
            var category = await _categoryService.GetCategoryById(data.CategoryId);

            existingMovie.Director = director;
            existingMovie.Category = category;

            if (data.Trailers != null && data.Trailers.Any())
            {
                var trailers = _movieService.CreateTrailers(data.Trailers, existingMovie);
                await _movieService.AddTrailersToMovie(existingMovie, trailers);
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
        [FromQuery] int? categoryId = null)
        {
            var movies = await _movieService.SearchMovies(name, releaseYear, categoryId);
            return Ok(movies);
        }
}
