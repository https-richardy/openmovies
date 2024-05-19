namespace OpenMovies.WebApi.Operations.Queries.Handlers;

public sealed class AuthenticationRequestHandler : IRequestHandler<AuthenticationRequest, AuthenticationResponse>
{
    private readonly IAccountService _accountService;
    private readonly IValidator<AuthenticationRequest> _validator;

    public AuthenticationRequestHandler(IAccountService accountService, IValidator<AuthenticationRequest> validator)
    {
        _accountService = accountService;
        _validator = validator;
    }

    public async Task<AuthenticationResponse> Handle(AuthenticationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var authenticationResponse = await _accountService.AuthenticateUserAsync(request);
        return authenticationResponse;
    }
}