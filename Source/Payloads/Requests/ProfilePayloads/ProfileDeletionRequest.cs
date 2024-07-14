namespace OpenMovies.WebApi.Payloads;

public sealed record ProfileDeletionRequest : AuthenticatedRequest, IRequest<Response>
{
    public int ProfileId { get; set; }
}