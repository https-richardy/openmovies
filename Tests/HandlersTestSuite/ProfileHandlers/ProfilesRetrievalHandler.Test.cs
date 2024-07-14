namespace OpenMovies.TestingSuite.HandlersTestSuite.ProfileHandlers;

public sealed class ProfilesRetrievalHandlerTest
{
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IProfileManager> _profileManagerMock;
    private readonly IRequestHandler<ProfilesRetrievalRequest, Response<IEnumerable<ProfileInformation>>> _handler;
    private readonly IServiceCollection _services;
    private readonly IFixture _fixture;

    public ProfilesRetrievalHandlerTest()
    {
        _userContextServiceMock = new Mock<IUserContextService>();
        _profileManagerMock = new Mock<IProfileManager>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _services = new ServiceCollection();
        _services.AddMapping(); // Add mapping settings with TinyMapper

        _handler = new ProfilesRetrievalHandler(
            userContextService: _userContextServiceMock.Object,
            profileManager: _profileManagerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid user, should return profiles successfully")]
    public async Task GivenValidUser_ShouldReturnProfilesSuccessfully()
    {
        var userId = Guid.NewGuid().ToString();
        var profiles = _fixture.CreateMany<Profile>(3).ToList();
        var formattedProfile = profiles.Select(profile => TinyMapper.Map<ProfileInformation>(profile)).ToList();

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _profileManagerMock.Setup(manager => manager.GetUserProfilesAsync(userId))
            .ReturnsAsync(profiles);

        var response = await _handler.Handle(new ProfilesRetrievalRequest(), CancellationToken.None);

        Assert.NotNull(response.Data);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Profiles successfully recovered.", response.Message);
        Assert.Equal(formattedProfile.Count, response.Data.Count());

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _profileManagerMock.Verify(manager => manager.GetUserProfilesAsync(userId), Times.Once);
    }

    [Fact(DisplayName = "Given a valid user with no profiles, should return empty list")]
    public async Task GivenValidUserWithNoProfiles_ShouldReturnEmptyList()
    {
        var userId = Guid.NewGuid().ToString();
        var profiles = new List<Profile>();

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _profileManagerMock.Setup(manager => manager.GetUserProfilesAsync(userId))
            .ReturnsAsync(profiles);

        var request = new ProfilesRetrievalRequest();
        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response.Data);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Profiles successfully recovered.", response.Message);
        Assert.Empty(response.Data);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _profileManagerMock.Verify(manager => manager.GetUserProfilesAsync(userId), Times.Once);
    }
}