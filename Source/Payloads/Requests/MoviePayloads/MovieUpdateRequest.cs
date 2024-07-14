namespace OpenMovies.WebApi.Payloads;

public sealed record MovieUpdateRequest : IRequest<Response>
{
    public string Title { get; init; }
    public string Synopsis { get; init; }
    public string VideoSource { get; init; }

    public int ReleaseYear { get; init; }
    public int DurationInMinutes { get; init; }
    public int CategoryId { get; init; }
    public int MovieId { get; set; }

    public IFormFile Image { get; init; }
}