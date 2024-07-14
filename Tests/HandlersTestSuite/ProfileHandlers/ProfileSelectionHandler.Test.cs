namespace OpenMovies.TestingSuite.HandlersTestSuite.ProfileHandlers;

public sealed class ProfileSelectionHandlerTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IProfileManager> _profileManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly IFixture _fixture;
    private readonly IRequestHandler<ProfileSelectionRequest, Response<AuthenticationResponse>> _handler;

    public ProfileSelectionHandlerTest()
    {
        #region Mocking
        #pragma warning disable CS8625
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

        _userContextServiceMock = new Mock<IUserContextService>();
        _profileManagerMock = new Mock<IProfileManager>();
        _jwtServiceMock = new Mock<IJwtService>();

        #endregion

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new ProfileSelectionHandler(
            userManager: _userManagerMock.Object,
            userContextService: _userContextServiceMock.Object,
            profileManager: _profileManagerMock.Object,
            jwtService: _jwtServiceMock.Object
        );
    }

    [Fact(DisplayName = "Given valid ProfileSelectionRequest, should return success response with token containing custom claims")]
    public async Task GivenValidProfileSelectionRequest_ShouldReturnSuccessResponseWithTokenContainingCustomClaims()
    {
        var userId = _fixture.Create<string>();
        var profileId = _fixture.Create<int>();

        var user = new ApplicationUser { Id = userId, UserName = "testuser", Email = "test@example.com" };
        var profile = new Profile { Id = profileId, Name = "Test Profile" };

        var request = new ProfileSelectionRequest { ProfileId = profileId };
        var roles = new List<string> { "Common" };

        _userContextServiceMock
            .Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileManagerMock
            .Setup(manager => manager.GetUserProfilesAsync(userId))
            .ReturnsAsync(new List<Profile> { profile });

        _userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(user))
            .ReturnsAsync(roles);

        var token = "mocked.jwt.token";

        _jwtServiceMock
            .Setup(service => service.GenerateToken(It.IsAny<ClaimsIdentity>()))
            .Returns(token);

        var result = await _handler.Handle(request, CancellationToken.None);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _profileManagerMock.Verify(manager => manager.GetUserProfilesAsync(userId), Times.Once);
        _userManagerMock.Verify(userManager => userManager.GetRolesAsync(user), Times.Once);
        _jwtServiceMock.Verify(service => service.GenerateToken(It.IsAny<ClaimsIdentity>()), Times.Once);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("Profile successfully selected.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(token, result.Data.Token);
    }

    [Fact(DisplayName = "Given non-existing profile id, should return not found response")]
    public async Task GivenNonExistingProfileId_ShouldReturnNotFoundResponse()
    {
        var userId = _fixture.Create<string>();
        var profileId = _fixture.Create<int>();

        var user = new ApplicationUser { Id = userId, UserName = "testuser", Email = "test@example.com" };
        var request = new ProfileSelectionRequest { ProfileId = profileId };

        _userContextServiceMock
            .Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileManagerMock
            .Setup(manager => manager.GetUserProfilesAsync(userId))
            .ReturnsAsync(new List<Profile>());

        var result = await _handler.Handle(request, CancellationToken.None);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _profileManagerMock.Verify(manager => manager.GetUserProfilesAsync(userId), Times.Once);

        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("Profile not found.", result.Message);
        Assert.Null(result.Data);
    }
}