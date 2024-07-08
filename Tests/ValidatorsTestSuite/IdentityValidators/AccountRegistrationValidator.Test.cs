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

    [Fact(DisplayName = "Given an empty password, should have validation error")]
    public async Task GivenEmptyPassword_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            Password = string.Empty
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Password is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a password with less than 8 characters, should have validation error")]
    public async Task GivenPasswordWithLessThan8Characters_ShouldHaveValidationError()
    {
        var payload = new AccountRegistrationRequest
        {
            Password = "1234567"
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Password must be at least 8 characters.", errorMessages);
    }

    [Theory(DisplayName = "Given an invalid password format, should have validation error")]
    [InlineData("password")]
    [InlineData("password123")]
    [InlineData("12345678")]
    [InlineData("Password123")]
    public async Task GivenInvalidPasswordFormat_ShouldHaveValidationError(string password)
    {
        var payload = new AccountRegistrationRequest
        {
            Password = password
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", errorMessages);
    }
}