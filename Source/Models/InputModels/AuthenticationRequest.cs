namespace OpenMovies.WebApi.Models.InputModels;

public record AuthenticationRequest : IRequest<AuthenticationResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}