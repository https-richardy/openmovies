namespace OpenMovies.WebApi.Models.ViewModels;

public record AuthenticationResponse
{
    public string Token { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }

    public static AuthenticationResponse FailureResponse() => new AuthenticationResponse
    {
        Success = false,
        Message = "invalid credentials."
    };

    public static AuthenticationResponse FailureResponse(string message) => new AuthenticationResponse
    {
        Success = false,
        Message = message
    };

    public static AuthenticationResponse SuccessResponse(string token) => new AuthenticationResponse
    {
        Success = true,
        Token = token,
        Message = "Authentication successful."
    };
}