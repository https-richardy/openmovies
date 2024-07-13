namespace OpenMovies.TestingSuite.ValidatorsTestSuite.ProfileValidators;

public sealed class ProfileCreationValidatorTest
{
    private readonly IValidator<ProfileCreationRequest> _validator;

    public ProfileCreationValidatorTest()
    {
        _validator = new ProfileCreationValidator();
    }

    [Fact(DisplayName = "Given an empty name, should have validation error")]
    public async Task GivenEmptyName_ShouldHaveValidationError()
    {
        var payload = new ProfileCreationRequest
        {
            Name = string.Empty,
            IsChild = true
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Name is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a name with less than 3 characters, should have validation error")]
    public async Task GivenNameWithLessThanThreeCharacters_ShouldHaveValidationError()
    {
        var payload = new ProfileCreationRequest
        {
            Name = "Jo",
            IsChild = true
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Name must be at least 3 characters.", errorMessages);
    }

    [Fact(DisplayName = "Given a valid payload, should not have validation error")]
    public async Task GivenValidPayload_ShouldNotHaveValidationError()
    {
        var payload = new ProfileCreationRequest
        {
            Name = "John Doe",
            IsChild = true
        };

        var result = await _validator.ValidateAsync(payload);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}