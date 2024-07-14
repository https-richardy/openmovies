namespace OpenMovies.TestingSuite.HandlersTestSuite.ProfileHandlers;

public sealed class ProfileDeletionHandlerTest
{
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IProfileManager> _profileManagerMock;
    private readonly ProfileDeletionHandler _handler;

    public ProfileDeletionHandlerTest()
    {
        _userContextServiceMock = new Mock<IUserContextService>();
        _profileManagerMock = new Mock<IProfileManager>();

        _handler = new ProfileDeletionHandler(
            userContextService: _userContextServiceMock.Object,
            profileManager: _profileManagerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid request and user, should delete profile successfully")]
    public async Task GivenValidRequestAndUser_ShouldDeleteProfileSuccessfully()
    {
        var request = new ProfileDeletionRequest
        {
            ProfileId = 1
        };

        var userId = Guid.NewGuid().ToString();

        _userContextServiceMock
            .Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        var deleteResult = OperationResult.Success("Profile deleted successfully.");

        _profileManagerMock
            .Setup(manager => manager.DeleteUserProfileAsync(userId, request.ProfileId))
            .ReturnsAsync(deleteResult);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Profile deleted successfully.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _profileManagerMock.Verify(manager => manager.DeleteUserProfileAsync(userId, request.ProfileId), Times.Once);
    }

    [Fact(DisplayName = "Given valid request and user, should return 404 Not Found if profile does not exist")]
    public async Task GivenValidRequestAndUser_ProfileNotFound_ShouldReturnNotFound()
    {
        var request = new ProfileDeletionRequest
        {
            ProfileId = 1
        };

        var userId = Guid.NewGuid().ToString();

        _userContextServiceMock
            .Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        var deleteResult = OperationResult.Failure("Profile not found.");

        _profileManagerMock
            .Setup(manager => manager.DeleteUserProfileAsync(userId, request.ProfileId))
            .ReturnsAsync(deleteResult);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("Profile not found.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _profileManagerMock.Verify(manager => manager.DeleteUserProfileAsync(userId, request.ProfileId), Times.Once);
    }
}