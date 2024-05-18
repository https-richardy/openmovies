namespace OpenMovies.WebApi.Services.Exceptions;

/// <summary>
/// Exception thrown when attempting to overwrite a file that already exists and overwriting is not allowed.
/// </summary>
public class FileOverwriteNotAllowedException : Exception
{
    public FileOverwriteNotAllowedException(string message) : base(message)
    {

    }
}