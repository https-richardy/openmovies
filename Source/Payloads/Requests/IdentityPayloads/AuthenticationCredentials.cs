namespace OpenMovies.WebApi.Payloads;

public sealed record AuthenticationCredentials
{
    public string Email { get; init; }
    public string Password { get; init; }
}