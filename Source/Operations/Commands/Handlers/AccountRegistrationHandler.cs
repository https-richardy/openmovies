namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class AccountRegistrationHandler : IRequestHandler<AccountRegistrationRequest, AccountRegistrationResponse>
{
    private readonly IAccountService _accountService;
    private readonly IValidator<AccountRegistrationRequest> _validator;

    public AccountRegistrationHandler(IAccountService accountService, IValidator<AccountRegistrationRequest> validator)
    {
        _accountService = accountService;
        _validator = validator;
    }

    public async Task<AccountRegistrationResponse> Handle(AccountRegistrationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var accountRegistrationResponse = await _accountService.RegisterUserAsync(request);
        return accountRegistrationResponse;
    }
}