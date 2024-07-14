namespace OpenMovies.TestingSuite.HandlersTestSuite.ProfileHandlers;

public sealed class ProfileEditingHandlerTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<IProfileManager> _profileManagerMock;
    private readonly Mock<IFileUploadService> _fileUploadServiceMock;
    private readonly Mock<IValidator<ProfileEditingRequest>> _validatorMock;
    private readonly IFixture _fixture;
    private readonly IServiceCollection _services;
    private readonly IRequestHandler<ProfileEditingRequest, Response> _handler;

    public ProfileEditingHandlerTest()
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
        _validatorMock = new Mock<IValidator<ProfileEditingRequest>>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _services = new ServiceCollection();
        _services.AddMapping(); // Add mapping settings with TinyMapper

        _handler = new ProfileEditingHandler(
            userManager: _userManagerMock.Object,
            userContextService: _userContextServiceMock.Object,
            profileManager: _profileManagerMock.Object,
            fileUploadService: _fileUploadServiceMock.Object,
            validator: _validatorMock.Object
        );

        #endregion
    }

    [Fact(DisplayName = "Given valid request and user, should update profile successfully")]
    public async Task GivenValidRequestAndUser_ShouldUpdateProfileSuccessfully()
    {
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(file => file.FileName).Returns("avatar.png");
        formFileMock.Setup(file => file.Length).Returns(100);
        formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

        var request = new ProfileEditingRequest
        {
            ProfileId = 1,
            Name = "Updated Name",
            IsChild = true,
            Avatar = formFileMock.Object
        };

        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId };

        var validationResult = new ValidationResult();

        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var existingProfile = new Profile { Id = request.ProfileId, Name = "Old Name" };

        _profileManagerMock.Setup(manager => manager.GetUserProfileByIdAsync(userId, request.ProfileId))
            .ReturnsAsync(existingProfile);

        _fileUploadServiceMock.Setup(service => service.UploadFileAsync(It.Is<IFormFile>(formFile => formFile == request.Avatar)))
            .ReturnsAsync("path/to/avatar.png");

        _profileManagerMock.Setup(manager => manager.UpdateUserProfileAsync(userId, It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Success("Profile updated successfully."));

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Profile updated successfully.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(request.Avatar), Times.Once);
        _profileManagerMock.Verify(manager => manager.UpdateUserProfileAsync(userId, It.IsAny<Profile>()), Times.Once);
    }

    [Fact(DisplayName = "Given valid request without avatar, should update profile successfully")]
    public async Task GivenValidRequestWithoutAvatar_ShouldUpdateProfileSuccessfully()
    {
        var request = new ProfileEditingRequest
        {
            ProfileId = 1,
            Name = "John Doe",
            IsChild = true,
            Avatar = null
        };

        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId };

        var validationResult = new ValidationResult();

        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var existingProfile = new Profile { Id = request.ProfileId, Name = "Old Name" };

        _profileManagerMock.Setup(manager => manager.GetUserProfileByIdAsync(userId, request.ProfileId))
            .ReturnsAsync(existingProfile);

        _profileManagerMock.Setup(manager => manager.UpdateUserProfileAsync(userId, It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Success("Profile updated successfully."));

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Profile updated successfully.", response.Message);

        _userContextServiceMock.Verify(service => service.GetCurrentUserId(), Times.Once);
        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
        _profileManagerMock.Verify(manager => manager.UpdateUserProfileAsync(userId, It.IsAny<Profile>()), Times.Once);
    }

    [Fact(DisplayName = "Given a invalid request should throw ValidationException")]
    public async Task GivenInvalidRequest_ShouldThrowValidationException()
    {
        var profile = _fixture.Create<Profile>();
        var request = _fixture.Build<ProfileEditingRequest>()
            .With(request => request.Avatar, new Mock<IFormFile>().Object)
            .Create();

        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId };

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required."),
        };

        var validationResult = new ValidationResult { Errors = validationErrors };

        _userContextServiceMock.Setup(service => service.GetCurrentUserId())
            .Returns(userId);

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileManagerMock.Setup(manager => manager.GetUserProfileByIdAsync(userId, request.ProfileId))
            .ReturnsAsync(profile);

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ThrowsAsync(new ValidationException(validationErrors));

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, default));
    }
}