namespace OpenMovies.WebApi.Models.InputModels;

public sealed class MovieCreationRequest
{
    public string Title { get; set; }
    public string Synopsis { get; set; }

    public int ReleaseYear { get; set; }
    public int CategoryId { get; set; }

    public IFormFile Cover { get; set; }
}