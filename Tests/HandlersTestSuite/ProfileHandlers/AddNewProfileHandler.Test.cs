namespace OpenMovies.TestingSuite.HandlersTestSuite.ProfileHandlers;

public sealed class AddNewProfileHandlerTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IProfileManager> _profileManagerMock;
    private readonly Mock<IFileUploadService> _fileUploadServiceMock;
    private readonly IFixture _fixture;
    private readonly IRequestHandler<ProfileCreationRequest, Response> _handler;
    private readonly int _maxNumberOfProfiles = 4;


    public AddNewProfileHandlerTest()
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
        _fileUploadServiceMock = new Mock<IFileUploadService>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new AddNewProfileHandler(
            userManager: _userManagerMock.Object,
            userContextService: _userContextServiceMock.Object,
            profileManager: _profileManagerMock.Object,
            fileUploadService: _fileUploadServiceMock.Object
        );

        #endregion
    }

    [Fact(DisplayName = "Given a valid user and valid avatar, should save the profile successfully")]
    public async Task GivenValidUserAndValidAvatar_ShouldSaveTheProfileSuccessfully()
    {
        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(file => file.FileName).Returns("avatar.png");
        formFileMock.Setup(file => file.Length).Returns(100);
        formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

        var request = new ProfileCreationRequest
        {
            Name = "John Doe",
            Avatar = formFileMock.Object
        };

        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId };

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _fileUploadServiceMock.Setup(service => service.UploadFileAsync(It.Is<IFormFile>(formFile => formFile == request.Avatar)))
            .ReturnsAsync("path/to/avatar.png");

        _profileManagerMock.Setup(manager => manager.SaveUserProfileAsync(It.Is<string>(userId => userId == user.Id), It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Success("Profile saved successfully."));

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal("Profile created successfully.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(request.Avatar), Times.Once);
        _profileManagerMock.Verify(manager => manager.SaveUserProfileAsync(userId, It.IsAny<Profile>()), Times.Once);
    }

    [Fact(DisplayName = "Given a valid user without avatar, should save the profile successfully")]
    public async Task GivenValidUserWithoutAvatar_ShouldSaveTheProfileSuccessfully()
    {
        var request = new ProfileCreationRequest
        {
            Name = "Test Profile",
            Avatar = null
        };

        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId };

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileManagerMock.Setup(manager => manager.SaveUserProfileAsync(It.Is<string>(id => id == user.Id), It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Success("Profile saved successfully."));

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal("Profile created successfully.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);

        /* because the avatar is null, the file upload service will not be called and the profileManager provides a default avatar. */
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
        _profileManagerMock.Verify(manager => manager.SaveUserProfileAsync(userId, It.IsAny<Profile>()), Times.Once);
    }

    [Fact(DisplayName = "Given user is not found, should return 404 Not Found")]
    public async Task GivenUserIsNotFound_ShouldReturnNotFound()
    {
        var request = new ProfileCreationRequest
        {
            Name = "Test Profile",
            Avatar = null
        };

        var userId = Guid.NewGuid().ToString();

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        #pragma warning disable CS8600 // disable the CS8600 warning because, in this scenario, the user must be null
        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync((ApplicationUser)null);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("User not found.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
        _profileManagerMock.Verify(manager => manager.SaveUserProfileAsync(It.IsAny<string>(), It.IsAny<Profile>()), Times.Never);
    }

        [Fact(DisplayName = "Given MaxProfileCountReachedException is thrown, should return 403 Forbidden")]
        public async Task GivenMaxProfileCountReachedExceptionIsThrown_ShouldReturnForbidden()
        {
            var request = new ProfileCreationRequest
            {
                Name = "Test Profile",
                Avatar = null
            };

            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser { Id = userId };

            _userContextServiceMock.Setup(service => service.GetCurrentUserId())
                .Returns(userId);

            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
                .ReturnsAsync(user);

            _profileManagerMock.Setup(manager => manager.SaveUserProfileAsync(It.Is<string>(id => id == user.Id), It.IsAny<Profile>()))
                .Throws(new MaxProfileCountReachedException(userId, _maxNumberOfProfiles));

            var response = await _handler.Handle(request, CancellationToken.None);

            Assert.Equal(StatusCodes.Status403Forbidden, response.StatusCode);
            Assert.Equal($"User with ID: {userId} has reached the maximum number of profiles allowed ({_maxNumberOfProfiles}).", response.Message);

            _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
            _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
            _fileUploadServiceMock.Verify(service => service.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
            _profileManagerMock.Verify(manager => manager.SaveUserProfileAsync(userId, It.IsAny<Profile>()), Times.Once);
        }
}