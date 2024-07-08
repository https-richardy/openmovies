namespace OpenMovies.TestingSuite.ValidatorsTestSuite.IdentityValidators;

public sealed class AuthenticationCredentialsValidatorTest
{
    private readonly IValidator<AuthenticationCredentials> _validator;

    public AuthenticationCredentialsValidatorTest()
    {
        _validator = new AuthenticationCredentialsValidator();
    }

    [Fact(DisplayName = "Given an empty email, should have validation error")]
    public async Task GivenEmptyEmail_ShouldHaveValidationError()
    {
        var payload = new AuthenticationCredentials
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
        var payload = new AuthenticationCredentials
        {
            Email = email
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Invalid email address format.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty password, should have validation error")]
    public async Task GivenEmptyPassword_ShouldHaveValidationError()
    {
        var payload = new AuthenticationCredentials
        {
            Password = string.Empty
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Password is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a valid payload, should not have validation error")]
    public async Task GivenValidPayload_ShouldNotHaveValidationError()
    {
        var payload = new AuthenticationCredentials
        {
            Email = "hOQpF@example.com",
            Password = "Password123!"
        };

        var result = await _validator.ValidateAsync(payload);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}