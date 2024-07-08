namespace OpenMovies.TestingSuite.ValidatorsTestSuite.IdentityValidators;

public sealed class AccountRegistrationValidatorTest
{
    private readonly IValidator<AccountRegistrationRequest> _validator;

    public AccountRegistrationValidatorTest()
    {
        _validator = new AccountRegistrationValidator();
    }

    [Fact(DisplayName = "Given an empty full name, should have validation error")]
    public async Task GivenEmptyFullName_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            FullName = string.Empty
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Full name is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a full name with less than 3 characters, should have validation error")]
    public async Task GivenFullNameWithLessThan3Characters_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            FullName = "ab"
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Full name must be at least 3 characters.", errorMessages);
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

    [Theory(DisplayName = "Given an invalid email format, should have validation error")]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("email.com")]
    public async Task GivenInvalidEmailFormat_ShouldHaveValidationError(string email)
    {
        var payload = new AccountRegistrationRequest
        {
            Email = email
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Invalid email address format.", errorMessages);
    }
}