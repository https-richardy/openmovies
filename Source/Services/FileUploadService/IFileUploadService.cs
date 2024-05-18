namespace OpenMovies.WebApi.Services;

public interface IFileUploadService
{
    /// <summary>
    /// Asynchronously uploads a file.
    /// </summary>
    /// <param name="file">The file to be uploaded.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the path of the uploaded file.
    /// </returns>
    /// <remarks>
    /// Implementations of this method should handle the asynchronous uploading of the provided file.
    /// The returned path should represent the location where the file has been uploaded.
    /// </remarks>
    Task<string> UploadFileAsync(IFormFile file);
}