namespace OpenMovies.WebApi.Models.ViewModels;

public record OperationResult
{
    public string Message { get; init; }
    public bool Success { get; init; }

    public static OperationResult SuccessResponse(string message) => new OperationResult
    {
        Success = true,
        Message = message,
    };

    public static OperationResult FailureResponse(string message) => new OperationResult
    {
        Success = false,
        Message = message
    };
}