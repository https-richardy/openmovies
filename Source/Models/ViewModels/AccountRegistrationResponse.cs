namespace OpenMovies.WebApi.Models.ViewModels;

public record AccountRegistrationResponse
{
    public bool Success { get; init; }
    public string Message { get; init; }

    public static AccountRegistrationResponse FailureResponse(string message) => new AccountRegistrationResponse
    {
        Success = false,
        Message = message
    };

    public static AccountRegistrationResponse SuccessResponse() => new AccountRegistrationResponse
    {
        Success = true,
        Message = "User created successfully."
    };
}