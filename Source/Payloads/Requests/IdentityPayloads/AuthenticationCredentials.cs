namespace OpenMovies.WebApi.Payloads;

public record AuthenticationCredentials
{
    public string Email { get; init; }
    public string Password { get; init; }
}