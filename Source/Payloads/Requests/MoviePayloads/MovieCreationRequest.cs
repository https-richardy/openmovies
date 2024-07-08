namespace OpenMovies.WebApi.Payloads;

public sealed record MovieCreationRequest : AuthenticatedRequest
{
    public string Title { get; init; }
    public string Synopsis { get; init; }
    public string VideoSource { get; init; }

    public int ReleaseYear { get; init; }
    public int DurationInMinutes { get; init; }
    public int CategoryId { get; init; }

    public IFormFile Image { get; init; }
}