namespace OpenMovies.TestingSuite.ValidatorsTestSuite.IdentityValidators;

public sealed class IdentityValidatorsExtensionTest
{
    private readonly IServiceCollection _services;

    public IdentityValidatorsExtensionTest()
    {
        _services = new ServiceCollection();
        _services.AddValidation();
    }

    [Fact(DisplayName = "Verify if AccountRegistrationValidator is registered.")]
    public void VerifyIfAccountRegistrationValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<AccountRegistrationRequest>>();

        Assert.NotNull(validator);
        Assert.IsType<AccountRegistrationValidator>(validator);
    }

    [Fact(DisplayName = "Verify if AuthenticationCredentialsValidator is registered.")]
    public void VerifyIfAuthenticationCredentialsValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<AuthenticationCredentials>>();

        Assert.NotNull(validator);
        Assert.IsType<AuthenticationCredentialsValidator>(validator);
    }
}