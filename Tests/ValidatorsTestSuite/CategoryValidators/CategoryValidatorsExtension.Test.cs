namespace OpenMovies.TestingSuite.ValidatorsTestSuite.CategoryValidators;

public sealed class CategoryValidatorsExtensionTest
{
    private readonly IServiceCollection _services;

    public CategoryValidatorsExtensionTest()
    {
        _services = new ServiceCollection();
        _services.AddValidation();
    }

    [Fact(DisplayName = "Verify if CategoryCreationValidator is registered.")]
    public void VerifyIfCategoryCreationValidatorIsRegistered()
    {
        var validator = _services.BuildServiceProvider()
            .GetRequiredService<IValidator<CategoryCreationRequest>>();

        Assert.NotNull(validator);
        Assert.IsType<CategoryCreationValidator>(validator);
    }
}