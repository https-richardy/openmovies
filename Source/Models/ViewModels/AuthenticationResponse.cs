namespace OpenMovies.WebApi.Models.ViewModels;

public record AuthenticationResponse : OperationResult
{
    public string Token { get; set; }

    public static new AuthenticationResponse SuccessResponse(string token) => new AuthenticationResponse
    {
        Success = true,
        Token = token
    };

    public static AuthenticationResponse InvalidCredentialsResponse() => new AuthenticationResponse
    {
        Success = false,
        Message = "invalid credentials."
    };
}