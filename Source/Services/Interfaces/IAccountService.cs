namespace OpenMovies.WebApi.Services;

public interface IAccountService
{
    Task<AccountRegistrationResponse> RegisterUserAsync(AccountRegistrationRequest request);
    Task<AuthenticationResponse> AuthenticateUserAsync(AuthenticationRequest request);
}