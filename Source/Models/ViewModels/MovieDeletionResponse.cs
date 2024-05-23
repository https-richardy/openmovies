namespace OpenMovies.WebApi.Models.ViewModels;

public sealed class MovieDeletionResponse
{
    public string Message { get; set; }

    public bool Success { get; set; }

    public static MovieDeletionResponse SuccessResponse() => new MovieDeletionResponse
    {
        Success = true,
        Message = "Movie deleted successfully."
    };

    public static MovieDeletionResponse FailureResponse(string message) => new MovieDeletionResponse
    {
        Success = false,
        Message = message
    };
}