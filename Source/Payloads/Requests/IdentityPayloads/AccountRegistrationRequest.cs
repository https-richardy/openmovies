namespace OpenMovies.WebApi.Payloads;

public record AccountRegistrationRequest : IRequest<Response>
{
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}