namespace OpenMovies.WebApi.Models.InputModels;

public record AccountRegistrationRequest : IRequest<OperationResult>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}