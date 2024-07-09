namespace OpenMovies.WebApi.Payloads;

public sealed class CategoryUpdateRequest : IRequest<Response>
{
    public string Name { get; set; }
}