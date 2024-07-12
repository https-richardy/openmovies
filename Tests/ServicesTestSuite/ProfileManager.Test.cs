namespace OpenMovies.TestingSuite.ServicesTestSuite;

public sealed class ProfileManagerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IProfileRepository> _profileRepositoryMock;
    private readonly Mock<IProfileCreationPolicy> _profileCreationPolicyMock;
    private readonly Mock<ILogger<ProfileManager>> _loggerMock;
    private readonly IFixture _fixture;
    private readonly IProfileManager _profileManager;

    public ProfileManagerTests()
    {
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

        _profileRepositoryMock = new Mock<IProfileRepository>();
        _profileCreationPolicyMock = new Mock<IProfileCreationPolicy>();
        _loggerMock = new Mock<ILogger<ProfileManager>>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _profileManager = new ProfileManager(
            _userManagerMock.Object,
            _profileRepositoryMock.Object,
            _profileCreationPolicyMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given a valid user and profile, should save the profile successfully")]
    public async Task GivenValidUserAndProfile_ShouldSaveTheProfileSuccessfully()
    {
        var userId = Guid.NewGuid().ToString();
        var profile = new Profile { Id = 1, Name = "Test Profile" };
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile>() };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileCreationPolicyMock.Setup(policy => policy.CanCreateProfileAsync(userId))
            .ReturnsAsync(true);
    
        _profileRepositoryMock.Setup(profileManager => profileManager.SaveAsync(profile))
            .ReturnsAsync(OperationResult.Success("Profile saved successfully."));

        _userManagerMock
            .Setup(userManager => userManager.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _profileManager.SaveUserProfileAsync(userId, profile);

        Assert.True(result.IsSuccess);
        Assert.Equal("Profile saved successfully.", result.Message);

        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _profileCreationPolicyMock.Verify(policy => policy.CanCreateProfileAsync(userId), Times.Once);

        _profileRepositoryMock.Verify(profileManager => profileManager.SaveAsync(profile), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(user), Times.Once);
    }

    [Fact(DisplayName = "Given a user with maximum profiles, should return failure")]
    public async Task GivenAUserWithMaximumProfiles_ShouldReturnFailure()
    {
        var profile = _fixture.Create<Profile>();
        var user = _fixture.Create<ApplicationUser>();

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.Is<string>(userId => userId == user.Id)))
            .ReturnsAsync(user);

        _profileCreationPolicyMock.Setup(policy => policy.CanCreateProfileAsync(It.Is<string>(userId => userId == user.Id)))
            .ReturnsAsync(false);

        var result = await _profileManager.SaveUserProfileAsync(userId: user.Id, profile: profile);

        Assert.False(result.IsSuccess);
        Assert.Equal("Maximum number of profiles reached.", result.Message);

        _profileRepositoryMock.Verify(repository => repository.SaveAsync(It.IsAny<Profile>()), Times.Never);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _profileCreationPolicyMock.Verify(policy => policy.CanCreateProfileAsync(It.Is<string>(userId => userId == user.Id)), Times.Once);
    }

    [Fact(DisplayName = "Given repository failure, should return failure")]
    public async Task GivenRepositoryFailure_ShouldReturnFailure()
    {
        var profile = _fixture.Create<Profile>();
        var user = _fixture.Create<ApplicationUser>();

        user.Profiles = new List<Profile>();

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _profileCreationPolicyMock.Setup(policy => policy.CanCreateProfileAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _profileRepositoryMock.Setup(repository => repository.SaveAsync(It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Failure("Repository error"));

        var result = await _profileManager.SaveUserProfileAsync(user.Id, profile);

        Assert.False(result.IsSuccess);
        Assert.Equal("Repository error", result.Message);

        _profileRepositoryMock.Verify(repository => repository.SaveAsync(It.IsAny<Profile>()), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _profileCreationPolicyMock.Verify(policy => policy.CanCreateProfileAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact(DisplayName = "Given a valid user and profile, should delete the profile successfully")]
    public async Task GivenValidUserAndProfile_ShouldDeleteTheProfileSuccessfully()
    {
        var userId = Guid.NewGuid().ToString();
        var profileId = 1;
        var profile = new Profile { Id = profileId, Name = "Test Profile" };
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile> { profile } };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileRepositoryMock.Setup(profileManager => profileManager.DeleteAsync(profile))
            .ReturnsAsync(OperationResult.Success("Profile deleted successfully."));

        _userManagerMock
            .Setup(userManager => userManager.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _profileManager.DeleteUserProfileAsync(userId, profileId);

        Assert.True(result.IsSuccess);
        Assert.Equal("Profile deleted successfully.", result.Message);

        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _profileRepositoryMock.Verify(profileManager => profileManager.DeleteAsync(profile), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(user), Times.Once);
    }

    [Fact(DisplayName = "Given a user and non-existent profile, should return failure")]
    public async Task GivenUserAndNonExistentProfile_ShouldReturnFailure()
    {
        var userId = Guid.NewGuid().ToString();
        var profileId = 1;
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile>() };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var result = await _profileManager.DeleteUserProfileAsync(userId, profileId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Message);

        _profileRepositoryMock.Verify(repository => repository.DeleteAsync(It.IsAny<Profile>()), Times.Never);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact(DisplayName = "Given repository failure when deleting profile, should return failure")]
    public async Task GivenRepositoryFailureWhenDeletingProfile_ShouldReturnFailure()
    {
        var userId = Guid.NewGuid().ToString();
        var profileId = 1;
        var profile = new Profile { Id = profileId, Name = "Test Profile" };
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile> { profile } };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileRepositoryMock.Setup(repository => repository.DeleteAsync(profile))
            .ReturnsAsync(OperationResult.Failure("Repository error"));

        var result = await _profileManager.DeleteUserProfileAsync(userId, profileId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Repository error", result.Message);

        _profileRepositoryMock.Verify(repository => repository.DeleteAsync(profile), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact(DisplayName = "Given a valid user and profile, should update the profile successfully")]
    public async Task GivenValidUserAndProfile_ShouldUpdateTheProfileSuccessfully()
    {
        var userId = Guid.NewGuid().ToString();
        var profile = new Profile { Id = 1, Name = "Updated Profile" };
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile> { profile } };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _profileRepositoryMock.Setup(profileManager => profileManager.UpdateAsync(profile))
            .ReturnsAsync(OperationResult.Success("Profile updated successfully."));

        _userManagerMock
            .Setup(userManager => userManager.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _profileManager.UpdateUserProfileAsync(userId, profile);

        Assert.True(result.IsSuccess);
        Assert.Equal("Profile updated successfully.", result.Message);

        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
        _profileRepositoryMock.Verify(profileManager => profileManager.UpdateAsync(profile), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(user), Times.Once);
    }

    [Fact(DisplayName = "Given repository failure when updating profile, should return failure")]
    public async Task GivenRepositoryFailureWhenUpdatingProfile_ShouldReturnFailure()
    {
        var profile = _fixture.Create<Profile>();
        var user = _fixture.Create<ApplicationUser>();

        user.Profiles = new List<Profile> { profile };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _profileRepositoryMock.Setup(repository => repository.UpdateAsync(It.IsAny<Profile>()))
            .ReturnsAsync(OperationResult.Failure("Repository error"));

        var result = await _profileManager.UpdateUserProfileAsync(user.Id, profile);

        Assert.False(result.IsSuccess);
        Assert.Equal("Repository error", result.Message);

        _profileRepositoryMock.Verify(repository => repository.UpdateAsync(It.IsAny<Profile>()), Times.Once);
        _userManagerMock.Verify(userManager => userManager.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact(DisplayName = "Given valid user and profileId, should return the profile")]
    public async Task GivenValidUserAndProfileId_ShouldReturnTheProfile()
    {
        var userId = Guid.NewGuid().ToString();
        var profileId = 1;
        var profile = new Profile { Id = profileId, Name = "Test Profile" };
        var user = new ApplicationUser { Id = userId, Profiles = new List<Profile> { profile } };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var result = await _profileManager.GetUserProfileByIdAsync(userId, profileId);

        Assert.NotNull(result);
        Assert.Equal(profileId, result?.Id);
        Assert.Equal("Test Profile", result?.Name);

        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
    }

    [Fact(DisplayName = "Given valid userId, should return user profiles")]
    public async Task GivenValidUserId_ShouldReturnUserProfiles()
    {
        var userId = Guid.NewGuid().ToString();
        var profiles = _fixture.CreateMany<Profile>(3).ToList();
        var user = new ApplicationUser { Id = userId, Profiles = profiles };

        _userManagerMock.Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(user);

        var result = await _profileManager.GetUserProfilesAsync(userId);

        Assert.Equal(profiles.Count, result.Count());
        Assert.Equal(profiles, result);

        _userManagerMock.Verify(userManager => userManager.FindByIdAsync(userId), Times.Once);
    }
}