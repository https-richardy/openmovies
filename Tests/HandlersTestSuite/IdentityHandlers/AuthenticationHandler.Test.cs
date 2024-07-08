namespace OpenMovies.TestingSuite.HandlersTestSuite.IdentityHandlers;

public sealed class AuthenticationHandlerTest
{
    private readonly Mock<IValidator<AuthenticationCredentials>> _validatorMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly IRequestHandler<AuthenticationCredentials, Response<AuthenticationResponse>> _handler;
    private readonly Fixture _fixture;

    public AuthenticationHandlerTest()
    {
        #pragma warning disable CS8625 // disable CS8625 because of Mocks they need to be null.
        #region Mocking
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            Mock.Of<IUserStore<IdentityUser>>(),
            null, /* passwordHasher */
            null, /* userValidators */
            null, /* passwordValidators */
            null, /* keyNormalizer */
            null, /* errors */
            null, /* services */
            null, /* logger */
            null  /* contextAccessor */
        );

        _jwtServiceMock = new Mock<IJwtService>();
        _validatorMock = new Mock<IValidator<AuthenticationCredentials>>();
        #endregion

        _handler = new AuthenticationHandler(
            userManager: _userManagerMock.Object,
            jwtService: _jwtServiceMock.Object,
            validator: _validatorMock.Object
        );

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given valid credentials, should return success response with token")]
    public async Task GivenValidCredentials_ShouldReturnSuccessResponseWithToken()
    {
        var credentials = new AuthenticationCredentials
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var user = new IdentityUser
        {
            Id = "123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        var roles = new List<string> { "Common" };

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AuthenticationCredentials>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult());

        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(credentials.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(user, credentials.Password))
            .ReturnsAsync(true);

        _userManagerMock.Setup(userManager => userManager.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtServiceMock.Setup(jwtService => jwtService.GenerateToken(It.IsAny<ClaimsIdentity>()))
            .Returns("mocked.jwt.token");

        var result = await _handler.Handle(credentials, CancellationToken.None);

        _validatorMock.Verify(validator => validator.ValidateAsync(
            It.IsAny<AuthenticationCredentials>(),
            It.IsAny<CancellationToken>()
        ));

        _userManagerMock.Verify(userManager => userManager.FindByEmailAsync(credentials.Email), Times.Once);
        _userManagerMock.Verify(userManager => userManager.CheckPasswordAsync(user, credentials.Password), Times.Once);
        _userManagerMock.Verify(userManager => userManager.GetRolesAsync(user), Times.Once);
        _jwtServiceMock.Verify(jwtService => jwtService.GenerateToken(It.IsAny<ClaimsIdentity>()), Times.Once);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("Authentication successful.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal("mocked.jwt.token", result.Data.Token);
    }

    [Fact(DisplayName = "Given invalid credentials, should return unauthorized response")]
    public async Task GivenInvalidCredentials_ShouldReturnUnauthorizedResponse()
    {
        var credentials = new AuthenticationCredentials
        {
            Email = "nonexistent@example.com",
            Password = "invalidpassword"
        };

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AuthenticationCredentials>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult());

        #pragma warning disable CS8600 // disable CS8600because it needs to be null since in this scenario no user is found.
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(credentials.Email))
            .ReturnsAsync((IdentityUser)null);

        var result = await _handler.Handle(credentials, CancellationToken.None);

        /* checking if the handler called the validator passing an AccountRegistrationRequest. */
        _validatorMock.Verify(validator => validator.ValidateAsync(
            It.IsAny<AuthenticationCredentials>(),
            It.IsAny<CancellationToken>()
        ));

        /* checking if the handler called the userManager passing an IdentityUser and a password. */
        _userManagerMock.Verify(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
        ));

        /*
            checking if the handler does not call the password verification method because in this scenario,
            since the user is null, it should return a NotAuthorized if the credentials are not found.
         */
        _userManagerMock.Verify(userManager => userManager.CheckPasswordAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>()
        ), Times.Never);

        _userManagerMock.Verify(userManager => userManager.GetRolesAsync(
            It.IsAny<IdentityUser>()
        ), Times.Never);

        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        Assert.Equal("Invalid email or password.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact(DisplayName = "Given invalid data, should throw validation exception")]
    public async Task GivenInvalidData_ShouldThrowValidationException()
    {
        var credentials = new AuthenticationCredentials
        {
            Email = "invalid"
        };

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AuthenticationCredentials>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new("Email", "Invalid email address format."),
                    new("Password", "Password is required.")
                }
            });

        await Assert.ThrowsAsync<ValidationException>(() =>_handler.Handle(credentials, CancellationToken.None));
    }
}