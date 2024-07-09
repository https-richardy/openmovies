namespace OpenMovies.TestingSuite.MediatorExtensionTestSuite;

public sealed class IdentityMediatorExtensionTest
{
    private readonly IServiceCollection _services;

    public IdentityMediatorExtensionTest()
    {
        _services = new ServiceCollection();
        _services.AddMediator();
    }

    [Fact(DisplayName = "Verify if AccountRegistrationHandler is registered.")]
    public void VerifyIfAccountRegistrationHandlerIsRegistered()
    {
        var handler = _services.BuildServiceProvider()
            .GetRequiredService<IRequestHandler<AccountRegistrationRequest, Response>>();

        Assert.NotNull(handler);
        Assert.IsType<AccountRegistrationHandler>(handler);
    }

    [Fact(DisplayName = "Verify if AuthenticationHandler is registered.")]
    public void VerifyIfAuthenticationHandlerIsRegistered()
    {
        var handler = _services.BuildServiceProvider()
            .GetRequiredService<IRequestHandler<AuthenticationCredentials, Response<AuthenticationResponse>>>();

        Assert.NotNull(handler);
        Assert.IsType<AuthenticationHandler>(handler);
    }
}