namespace OpenMovies.WebApi.Exceptions;

/// <summary>
/// Exception thrown when attempting to register a user that already exists.
/// </summary>
public class UserAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAlreadyExistsException"/> class.
    /// </summary>
    public UserAlreadyExistsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UserAlreadyExistsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAlreadyExistsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public UserAlreadyExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}