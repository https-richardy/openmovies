namespace OpenMovies.WebApi.Payloads;

public sealed record CategoryRetrievalRequest : IRequest<Response<Category>>
{
    public int CategoryId { get; set; }
}