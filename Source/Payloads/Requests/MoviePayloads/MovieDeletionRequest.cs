namespace OpenMovies.WebApi.Payloads;

public sealed record MovieDeletionRequest : IRequest<Response>
{
    public int MovieId { get; set; }
}