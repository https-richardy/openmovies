namespace OpenMovies.WebApi.Payloads;

public sealed class CategoryUpdateRequest : IRequest<Response>
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
}