namespace OpenMovies.WebApi.Services.Exceptions;

/// <summary>
/// Exception thrown when the file extension is not allowed.
/// </summary>
public class InvalidFileExtensionException : Exception
{
    public InvalidFileExtensionException(string message) : base(message)
    {

    }
}