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

    public MovieController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    // [HttpPost]
    // public async Task<IActionResult> Create(CreateMovieDTO data)
    // {
        // var movie = new Movie(data.Title, data.ReleaseDateOf, data.Synopsis, director, category);

        // await _movieService.CreateMovie(movie);

        // return StatusCode(201, movie);
    // }
}
