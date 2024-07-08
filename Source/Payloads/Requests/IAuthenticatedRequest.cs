namespace OpenMovies.WebApi.Payloads;

public interface IAuthenticatedRequest
{
    string UserId { get; }
}