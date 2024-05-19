namespace OpenMovies.WebApi.Models.ViewModels;

public sealed record MovieCreationResponse
{
    public string Message { get; set; }

    public bool Success { get; set; }

    public static MovieCreationResponse SuccessResponse() => new MovieCreationResponse
    {
        Success = true,
        Message = "Movie created successfully."
    };

    public static MovieCreationResponse FailureResponse(string message) => new MovieCreationResponse
    {
        Success = false,
        Message = message
    };
} 