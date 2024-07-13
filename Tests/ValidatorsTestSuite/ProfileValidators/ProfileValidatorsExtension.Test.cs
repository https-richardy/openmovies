namespace OpenMovies.TestingSuite.ValidatorsTestSuite.ProfileValidators;

public sealed class ProfileValidatorsExtensionTest
{
    private readonly IServiceCollection _services;

    public ProfileValidatorsExtensionTest()
    {
        _services = new ServiceCollection();
        _services.AddValidation();
    }

    [Fact(DisplayName = "Should verify if ProfileCreationValidator is registered.")]
    public void ShouldVerifyIProfileCreationValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<ProfileCreationValidator>>();

        Assert.NotNull(validator);
        Assert.IsType<ProfileCreationValidator>(validator);
    }

    [Fact(DisplayName = "Should verify if ProfileEditingValidator is registered.")]
    public void ShouldVerifyIfProfileEditingValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<ProfileEditingRequest>>();

        Assert.NotNull(validator);
        Assert.IsType<ProfileEditingValidator>(validator);
    }
}