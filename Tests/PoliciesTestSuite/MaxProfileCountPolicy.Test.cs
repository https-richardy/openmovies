namespace OpenMovies.TestingSuite.PoliciesTestSuite;

public sealed class MaxProfileCountPolicyTest
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IProfileCreationPolicy _policy;
    private const int _currentMaxNumberOfProfilesPerAccount = 4;
    private readonly IFixture _fixture;

    public MaxProfileCountPolicyTest()
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
        #endregion

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _policy = new MaxProfileCountPolicy(userManager: _userManagerMock.Object);
    }

    [Fact(DisplayName = "Users with fewer profiles than the maximum limit can create a new profile")]
    public async Task UserWithLessThanMaxProfilesCanCreateNewProfile()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Profiles = new List<Profile>() };

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(It.Is<string>(userId => userId == user.Id)))
            .ReturnsAsync(user);

        var result = await _policy.CanCreateProfileAsync(user.Id);
        Assert.True(result);
    }

    [Fact(DisplayName = "Users with exactly the maximum number of profiles cannot create a new profile")]
    public async void UserWithExactlyMaxProfilesCannotCreateNewProfile()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        var profiles = _fixture.Build<Profile>()
            .With(profile => profile.Account, user)
            .CreateMany(_currentMaxNumberOfProfilesPerAccount)
            .ToList();

        user.Profiles = profiles;

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(It.Is<string>(userId => userId == user.Id)))
            .ReturnsAsync(user);

        var result = await _policy.CanCreateProfileAsync(user.Id);
        Assert.False(result);
    }

    [Fact(DisplayName = "Users with more profiles than the maximum limit cannot create a new profile")]
    public async Task UserWithMoreThanMaxProfilesCannotCreateNewProfile()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        var profiles = _fixture.Build<Profile>()
            .With(profile => profile.Account, user)
            .CreateMany(_currentMaxNumberOfProfilesPerAccount + 2)
            .ToList();

        user.Profiles = profiles;

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(It.Is<string>(userId => userId == user.Id)))
            .ReturnsAsync(user);

        var result = await _policy.CanCreateProfileAsync(user.Id);
        Assert.False(result);
    }
}