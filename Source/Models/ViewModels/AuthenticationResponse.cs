namespace OpenMovies.WebApi.Models.ViewModels;

public record AuthenticationResponse : OperationResult
{
    public string Token { get; set; }

    public static AuthenticationResponse InvalidCredentialsResponse() => new AuthenticationResponse
    {
        Success = false,
        Message = "invalid credentials."
    };
}