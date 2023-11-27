namespace OpenMovies.DTOs;

public class MovieDTO
{
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDateOf { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    public List<TrailerDTO>? Trailers { get; set; } = new List<TrailerDTO>();
    public int DirectorId { get; set; }
    public int CategoryId { get; set; }
}