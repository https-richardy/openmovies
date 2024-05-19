namespace OpenMovies.WebApi.Models.InputModels;

public sealed record UpdateMovieRequest
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public int ReleaseYear { get; set; }

    public MovieCategory Category { get; set; }
    public IFormFile Cover { get; set; }
    public Duration Duration { get; set; }
}