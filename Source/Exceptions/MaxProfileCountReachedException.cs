namespace OpenMovies.WebApi.Exceptions;

public sealed class MaxProfileCountReachedException : Exception
{
    public MaxProfileCountReachedException(string userId, int maxNumberOfProfiles)
        : base($"User with ID: {userId} has reached the maximum number of profiles allowed ({maxNumberOfProfiles}).")
    {
        
    }
}
