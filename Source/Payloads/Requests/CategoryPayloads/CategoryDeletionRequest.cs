namespace OpenMovies.WebApi.Payloads;

public sealed record CategoryDeletionRequest : IRequest<Response>
{
    public int CategoryId { get; set; }
}