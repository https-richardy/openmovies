namespace OpenMovies.TestingSuite.ValidatorsTestSuite.MovieValidators;

public sealed class MovieValidatorsExtensionTest
{
    private readonly IServiceCollection _services;

    public MovieValidatorsExtensionTest()
    {
        _services = new ServiceCollection();
        _services.AddValidation();
    }

    [Fact(DisplayName = "Should verify if MovieCreationValidator is registered.")]
    public void ShouldVerifyIfMovieCreationValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<MovieCreationRequest>>();

        Assert.NotNull(validator);
        Assert.IsType<MovieCreationValidator>(validator);
    }

    [Fact(DisplayName = "Should verify if MovieUpdateValidator is registered.")]
    public void ShouldVerifyIfMovieUpdateValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<MovieUpdateRequest>>();

        Assert.NotNull(validator);
        Assert.IsType<MovieUpdateValidator>(validator);
    }
}