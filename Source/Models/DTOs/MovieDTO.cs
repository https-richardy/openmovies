# pragma warning disable CS8618

namespace OpenMovies.DTOs;

public class MovieDTO
{
    public string Title { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public int CategoryId { get; set; }

    public IFormFile Cover { get; set; }
    public DateTime ReleaseDateOf { get; set; }
}