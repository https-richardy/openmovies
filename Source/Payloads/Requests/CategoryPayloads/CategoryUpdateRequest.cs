namespace OpenMovies.WebApi.Payloads;

public sealed class CategoryUpdateRequest : IRequest<Response>
{
    [JsonIgnore]
    public int CategoryId { get; set; }
    public string Name { get; set; }
}