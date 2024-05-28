namespace OpenMovies.WebApi.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an object does not exist.
/// </summary>
public class ObjectDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectDoesNotExistException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ObjectDoesNotExistException(string message) : base(message)
    {

    }
}