using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OpenMovies.DTOs;
using OpenMovies.Models;
using OpenMovies.Services;

namespace OpenMovies.Controllers;

[ApiController]
[Route("api/directors")]
public class DirectorController : ControllerBase
{
    private readonly DirectorService _directorService;

    public DirectorController(DirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var directors = await _directorService.GetAllDirectors();
        return Ok(directors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var retrievedDirector = await _directorService.GetDirectorById(id);
            return Ok(retrievedDirector);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(DirectorDTO data)
    {
        try
        {
            var director = new Director(data.FirstName, data.LastName);
            await _directorService.CreateDirector(director);

            return StatusCode(201, director);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DirectorDTO data)
    {
        try
        {
            var retrievedDirector = await _directorService.GetDirectorById(id);
    
            retrievedDirector.FirstName = data.FirstName;
            retrievedDirector.LastName = data.LastName;

            await _directorService.UpdateDirector(retrievedDirector);

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
}
