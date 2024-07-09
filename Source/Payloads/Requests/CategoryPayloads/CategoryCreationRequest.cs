namespace OpenMovies.WebApi.Payloads;

public sealed record CategoryCreationRequest : IRequest<Response>
{
    public string Name { get; init; }
}