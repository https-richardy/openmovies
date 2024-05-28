namespace OpenMovies.WebApi.Services;

public interface IAccountService
{
    Task<OperationResult> RegisterUserAsync(AccountRegistrationRequest request);
    Task<AuthenticationResponse> AuthenticateUserAsync(AuthenticationRequest request);
}