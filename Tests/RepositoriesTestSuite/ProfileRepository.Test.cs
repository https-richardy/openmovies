namespace OpenMovies.TestingSuite.RepositoriesTestSuite;

public sealed class ProfileRepositoryTest : InMemoryDatabaseFixture<AppDbContext>
{
    private readonly IProfileRepository _profileRepository;
    private readonly Mock<ILogger<ProfileRepository>> _loggerMock;

    public ProfileRepositoryTest()
    {
        _loggerMock = new Mock<ILogger<ProfileRepository>>();

        _profileRepository = new ProfileRepository(
            dbContext: DbContext,
            logger: _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given a valid profile, should save successfully in the database")]
    public async Task GivenValidProfile_ShouldSaveSuccessfullyInTheDatabase()
    {
        var profile = Fixture.Create<Profile>();

        var result = await _profileRepository.SaveAsync(profile);
        var savedProfile = await DbContext.Profiles.FindAsync(profile.Id);

        Assert.NotNull(savedProfile);
        Assert.True(result.IsSuccess);

        Assert.Equal(profile.Id, savedProfile.Id);
        Assert.Equal(profile.Account.Id, savedProfile.Account.Id);

        Assert.Equal(profile.Name, savedProfile.Name);
        Assert.Equal(profile.Avatar, savedProfile.Avatar);
        Assert.Equal(profile.BookmarkedMovies.Count, savedProfile.BookmarkedMovies.Count);
        Assert.Equal(profile.WatchedMovies.Count, savedProfile.WatchedMovies.Count);
    }

    [Fact(DisplayName = "Given a valid profile, should update successfully in the database")]
    public async Task GivenValidProfile_ShouldUpdateSuccessfullyInTheDatabase()
    {
        var profile = Fixture.Create<Profile>();

        await DbContext.Profiles.AddAsync(profile);
        await DbContext.SaveChangesAsync();

        profile.Name = "John Doe";
        profile.Avatar = "uploads/john-doe.png";

        var result = await _profileRepository.UpdateAsync(profile);
        var updatedProfile = await DbContext.Profiles.FindAsync(profile.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedProfile);

        Assert.Equal(profile.Name, updatedProfile.Name);
        Assert.Equal(profile.Avatar, updatedProfile.Avatar);
    }

    [Fact(DisplayName = "Given a valid profile, should delete successfully from the database")]
    public async Task GivenValidProfile_ShouldDeleteSuccessfullyFromTheDatabase()
    {
        var profile = Fixture.Create<Profile>();

        await DbContext.Profiles.AddAsync(profile);
        await DbContext.SaveChangesAsync();

        var result = await _profileRepository.DeleteAsync(profile);
        var deletedProfile = await DbContext.Profiles.FindAsync(profile.Id);

        Assert.True(result.IsSuccess);
        Assert.Null(deletedProfile);
    }

    [Fact(DisplayName = "Given a valid predicate, should find all matching profiles")]
    public async Task GivenValidPredicate_ShouldFindAllMatchingProfiles()
    {
        const string profileNameToSearch = "John Doe";
        var profiles = Fixture.CreateMany<Profile>(5).ToList();

        profiles[0].Name = profileNameToSearch;
        profiles[1].Name = profileNameToSearch;

        await DbContext.Profiles.AddRangeAsync(profiles);
        await DbContext.SaveChangesAsync();

        var foundProfiles = await _profileRepository.FindAllAsync(profile => profile.Name == profileNameToSearch);

        Assert.Equal(2, foundProfiles.Count());
    }

    [Fact(DisplayName = "Given a valid predicate, should find a single profile")]
    public async Task GivenValidPredicate_ShouldFindSingleProfile()
    {
        var profile = Fixture.Create<Profile>();

        await DbContext.Profiles.AddAsync(profile);
        await DbContext.SaveChangesAsync();

        var foundProfile = await _profileRepository.FindSingleAsync(profile => profile.Id == profile.Id);

        Assert.NotNull(foundProfile);

        Assert.Equal(profile.Id, foundProfile.Id);
        Assert.Equal(profile.Name, foundProfile.Name);
        Assert.Equal(profile.Avatar, foundProfile.Avatar);

        Assert.Equal(profile.BookmarkedMovies.Count, foundProfile.BookmarkedMovies.Count);
        Assert.Equal(profile.WatchedMovies.Count, foundProfile.WatchedMovies.Count);
    }

    [Fact(DisplayName = "Should fetch all profiles")]
    public async Task ShouldFetchAllProfiles()
    {
        var profiles = Fixture.CreateMany<Profile>(5).ToList();

        await DbContext.Profiles.AddRangeAsync(profiles);
        await DbContext.SaveChangesAsync();

        var foundProfiles = await _profileRepository.GetAllAsync();

        Assert.Equal(profiles.Count, foundProfiles.Count());
    }

    [Fact(DisplayName = "Given a valid valid id, should fetch a profile by id")]
    public async Task GivenValidValidId_ShouldFetchProfileById()
    {
        var profile = Fixture.Create<Profile>();

        await DbContext.Profiles.AddAsync(profile);
        await DbContext.SaveChangesAsync();

        var foundProfile = await _profileRepository.GetByIdAsync(profile.Id);

        Assert.NotNull(foundProfile);
        Assert.Equal(profile.Id, foundProfile.Id);
        Assert.Equal(profile.Name, foundProfile.Name);
        Assert.Equal(profile.Avatar, foundProfile.Avatar);

        Assert.Equal(profile.BookmarkedMovies.Count, foundProfile.BookmarkedMovies.Count);
        Assert.Equal(profile.WatchedMovies.Count, foundProfile.WatchedMovies.Count);
    }

    [Fact(DisplayName = "Given a valid predicate, should fetch all matching profiles paginated")]
    public async Task GivenValidPredicate_ShouldFetchAllMatchingProfilesPaginated()
    {
        const string profileNameToSearch = "John Doe";
        var profiles = Fixture.CreateMany<Profile>(25).ToList();

        profiles[0].Name = profileNameToSearch;
        profiles[1].Name = profileNameToSearch;
        profiles[2].Name = profileNameToSearch;

        await DbContext.Profiles.AddRangeAsync(profiles);
        await DbContext.SaveChangesAsync();

        const int pageNumber = 1;
        const int pageSize = 5;

        var pagedProfiles = await _profileRepository.PagedAsync(profile => profile.Name == profileNameToSearch, pageNumber, pageSize);

        Assert.Equal(3, pagedProfiles.Count());
    }

    [Fact(DisplayName = "GetBookmarkedMoviesAsync should return correct movies for profile")]
    public async Task GetBookmarkedMoviesAsync_ShouldReturnCorrectMoviesForProfile()
    {
        var profile = Fixture.Create<Profile>();
        profile.BookmarkedMovies.Clear();

        var bookmarkedMovies = Fixture.Build<BookmarkedMovie>()
            .With(bookmarkedMovies => bookmarkedMovies.Profile, profile)
            .CreateMany(3);

        await DbContext.BookmarkedMovies.AddRangeAsync(bookmarkedMovies);
        await DbContext.SaveChangesAsync();

        var result = await _profileRepository.GetBookmarkedMoviesAsync(profile);

        Assert.Equal(bookmarkedMovies.Count(), result.Count());

        foreach (var movie in result)
        {
            Assert.Equal(profile.Id, movie.Profile.Id);
        }
    }
}