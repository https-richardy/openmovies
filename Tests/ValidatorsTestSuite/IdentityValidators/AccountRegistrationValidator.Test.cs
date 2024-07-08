namespace OpenMovies.TestingSuite.ValidatorsTestSuite.IdentityValidators;

public sealed class AccountRegistrationValidatorTest
{
    private readonly IValidator<AccountRegistrationRequest> _validator;

    public AccountRegistrationValidatorTest()
    {
        _validator = new AccountRegistrationValidator();
    }

    [Fact(DisplayName = "Given an empty email, should have validation error")]
    public async Task GivenEmptyEmail_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            Email = string.Empty
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Email is required.", errorMessages);
    }

    [Fact(DisplayName = "Given an invalid email format, there must be a validation error")]
    public async Task GivenInvalidEmailFormat_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            Email = "invalid"
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Invalid email address format.", errorMessages);
    }
}