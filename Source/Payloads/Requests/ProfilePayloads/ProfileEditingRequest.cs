namespace OpenMovies.WebApi.Payloads;

public sealed record ProfileEditingRequest : AuthenticatedRequest, IRequest<Response>
{
    public string Name { get; init; }
    public bool IsChild { get; init; }
    public IFormFile? Avatar { get; init; }

    [JsonIgnore]
    public int ProfileId { get; set; }
}