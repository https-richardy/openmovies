namespace OpenMovies.WebApi.Models.ViewModels;

public record AuthenticationResponse
{
    public string Token { get; set; }
}