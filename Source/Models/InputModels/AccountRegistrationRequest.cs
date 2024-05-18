namespace OpenMovies.WebApi.Models.ViewModels;

public record AccountRegistrationRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}