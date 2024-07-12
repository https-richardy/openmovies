namespace OpenMovies.TestingSuite.HandlersTestSuite.IdentityHandlers;

public sealed class AccountRegistrationHandlerTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<IValidator<AccountRegistrationRequest>> _validatorMock;
    private readonly Mock<IProfileManager> _profileManager;
    private readonly IRequestHandler<AccountRegistrationRequest, Response> _handler;
    private readonly IFixture _fixture;

    public AccountRegistrationHandlerTest()
    {
        #pragma warning disable CS8625 // disable CS8625 because of Mocks they need to be null.
        #region Mocks
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, /* passwordHasher */
            null, /* userValidators */
            null, /* passwordValidators */
            null, /* keyNormalizer */
            null, /* errors */
            null, /* services */
            null, /* logger */
            null  /* contextAccessor */
        );

        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(),
            null, /* roleValidators */
            null, /* keyNormalizer */
            null, /* errors */
            null  /* logger */
        );

        _profileManager = new Mock<IProfileManager>();
        _validatorMock = new Mock<IValidator<AccountRegistrationRequest>>();
        #endregion

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new AccountRegistrationHandler(
            userManager: _userManagerMock.Object,
            roleManager: _roleManagerMock.Object,
            profileManager: _profileManager.Object,
            validator: _validatorMock.Object
        );
    }

    [Fact(DisplayName = "Given a valid request, should return success response")]
    public async Task GivenValidRequest_ShouldReturnSuccessResponse()
    {
        var request = _fixture.Create<AccountRegistrationRequest>();

        #region setup the behavior of mocks in this scenario. 
        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AccountRegistrationRequest>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult());

        _userManagerMock.Setup(userManager => userManager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(IdentityResult.Success);

        _profileManager.Setup(profileManager => profileManager.SaveUserProfileAsync(
                It.IsAny<string>(),
                It.IsAny<Profile>()
            ))
            .ReturnsAsync(OperationResult.Success());
        #endregion

        var result = await _handler.Handle(request, CancellationToken.None);

        /* checking if the handler called the validator passing an AccountRegistrationRequest. */
        _validatorMock.Verify(validator => validator.ValidateAsync(
            It.IsAny<AccountRegistrationRequest>(),
            It.IsAny<CancellationToken>()
        ));

        /* checking if the handler called the userManager passing an ApplicationUser and a password. */
        _userManagerMock.Verify(userManager => userManager.CreateAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<string>()
        ));

        /* checking if the handler called the userManager passing an ApplicationUser and a role. */
        _userManagerMock.Verify(userManager => userManager.AddToRoleAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<string>()
        ));

        /* checking if the handler called the profileManager passing an ApplicationUser. */
        _profileManager.Verify(profileManager => profileManager.SaveUserProfileAsync(
            It.IsAny<string>(),
            It.IsAny<Profile>()
        ));

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        Assert.Equal("Account registration successful.", result.Message);
    }

    [Fact(DisplayName = "Given an invalid request, should return error response")]
    public async Task GivenInvalidRequest_ShouldReturnErrorResponse()
    {
        var request = _fixture.Create<AccountRegistrationRequest>();

        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AccountRegistrationRequest>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new("Email", "Invalid email address format.")
                }
            });

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact(DisplayName = "Given a request, should create role 'Common' if it doesn't exist")]
    public async Task GivenRequest_ShouldCreateRoleIfItDoesntExist()
    {
        var request = _fixture.Create<AccountRegistrationRequest>();

        #region setup the behavior of mocks in this scenario.
        /* setting up the behavior of the validator. In this scenario, the validator will return a success result. */
        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AccountRegistrationRequest>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult());

        /* setting up the behavior of the userManager. In this scenario, the userManager will return a success result. */
        _userManagerMock.Setup(userManager => userManager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(IdentityResult.Success);

        /* setting up the behavior of the roleManager.RoleExistsAsync. In this scenario, the roleManager will return a false result. */
        _roleManagerMock.Setup(roleManager => roleManager.RoleExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        /* setting up the behavior of the roleManager.CreateAsync. In this scenario, the roleManager will return a success result. */
        _roleManagerMock.Setup(roleManager => roleManager.CreateAsync(It.IsAny<IdentityRole>()))
            .ReturnsAsync(IdentityResult.Success);
        #endregion

        var result = await _handler.Handle(request, CancellationToken.None);

        _roleManagerMock.Verify(roleManager => roleManager.RoleExistsAsync(It.IsAny<string>()), Times.Once);
        _roleManagerMock.Verify(roleManager => roleManager.CreateAsync(It.IsAny<IdentityRole>()), Times.Once);
    }

    [Fact(DisplayName = "Given a request, should add user to 'Common' role")]
    public async Task GivenRequest_ShouldAddUserToCommonRole()
    {
        var request = _fixture.Create<AccountRegistrationRequest>();

        #region setup the behavior of mocks in this scenario.
        /* setting up the behavior of the validator. In this scenario, the validator will return a success result. */
        _validatorMock.Setup(validator => validator.ValidateAsync(
                It.IsAny<AccountRegistrationRequest>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ValidationResult());

        /* setting up the behavior of the userManager. In this scenario, the userManager will return a success result. */
        _userManagerMock.Setup(userManager => userManager.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(IdentityResult.Success);

        /* setting up the behavior of the roleManager.RoleExistsAsync. In this scenario, the roleManager will return a true result. */
        _roleManagerMock.Setup(roleManager => roleManager.RoleExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        /* setting up the behavior of the userManager.AddToRoleAsync. In this scenario, the userManager will return a success result. */
        _userManagerMock.Setup(userManager => userManager.AddToRoleAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(IdentityResult.Success);
        #endregion

        var result = await _handler.Handle(request, CancellationToken.None);

        /* verify that AddToRoleAsync was called once with the expected parameters. */
        _userManagerMock.Verify(userManager => userManager.AddToRoleAsync(
            It.Is<ApplicationUser>(u => u.Email == request.Email),
            "Common"
        ), Times.Once);
    }
}