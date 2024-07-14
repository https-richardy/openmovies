namespace OpenMovies.WebApi.Payloads;

public sealed record AuthenticationResponse
{
    public string Token { get; init; }
}