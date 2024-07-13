namespace OpenMovies.WebApi.Payloads;

public sealed record ProfilesRetrievalRequest
    : AuthenticatedRequest, IRequest<IEnumerable<ProfileInformation>>
{
    /* inherits user id from AuthenticatedRequest record. */
}