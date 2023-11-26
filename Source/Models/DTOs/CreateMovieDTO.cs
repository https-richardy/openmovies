using System.ComponentModel.DataAnnotations;
using OpenMovies.Models;

namespace OpenMovies.DTOs;


public class CreateMovieDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDateOf { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 60)]
    public string Synopsis { get; set; } = string.Empty;

    public List<TrailerDTO>? Trailers { get; set; } = new List<TrailerDTO>();

    public int DirectorId { get; set; }
    public int CategoryId { get; set; }
}