namespace OpenMovies.WebApi.Payloads;

public sealed record ProfileSelectionRequest
    : AuthenticatedRequest, IRequest<Response<AuthenticationResponse>>
{
    public int ProfileId { get; set; }
}