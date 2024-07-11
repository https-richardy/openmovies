namespace OpenMovies.WebApi.Payloads;

public sealed record CategoryRetrievalRequest : IRequest<Response>
{
    public int CategoryId { get; set; }
}