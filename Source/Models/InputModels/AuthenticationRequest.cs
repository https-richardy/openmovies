namespace OpenMovies.WebApi.Models.InputModels;

public record AuthenticationRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}