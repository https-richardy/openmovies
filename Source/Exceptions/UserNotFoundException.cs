namespace OpenMovies.WebApi.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId)
        : base($"User with ID: {userId} not found.")
    {
    }
}
