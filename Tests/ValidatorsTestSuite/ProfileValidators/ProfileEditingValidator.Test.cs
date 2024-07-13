namespace OpenMovies.TestingSuite.ValidatorsTestSuite.ProfileValidators;

public sealed class ProfileEditingValidatorTest
{
    private readonly IValidator<ProfileEditingRequest> _validator;

    public ProfileEditingValidatorTest()
    {
        _validator = new ProfileEditingValidator();
    }

    [Fact(DisplayName = "Given an empty name, should have validation error")]
    public async Task GivenEmptyName_ShouldHaveValidationError()
    {
        var payload = new ProfileEditingRequest
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
        var payload = new ProfileEditingRequest
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
        var payload = new ProfileEditingRequest
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