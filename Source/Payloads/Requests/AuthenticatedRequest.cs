namespace OpenMovies.WebApi.Payloads;

public record AuthenticatedRequest : IAuthenticatedRequest
{
    [JsonIgnore]
    public string UserId { get; set; }
}