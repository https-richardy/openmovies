namespace OpenMovies.WebApi.Data.Repositories;

public sealed class ProfileRepository(
    AppDbContext dbContext,
    ILogger<ProfileRepository> logger
) : Repository<Profile, AppDbContext>(dbContext, logger), IProfileRepository
{
    public async Task<IEnumerable<BookmarkedMovie>> GetBookmarkedMoviesAsync(Profile profile)
    {
        Logger.LogInformation("Fetching for movies bookmarked to the profile: {profileId}", profile.Id);

        var bookmarkedMovies = await DbContext.BookmarkedMovies
            .Include(bookmarkedMovie => bookmarkedMovie.Profile)
            .Where(bookmarkedMovie => bookmarkedMovie.Profile.Id == profile.Id)
            .ToListAsync();

        return bookmarkedMovies;
    }

    public async Task<Profile?> GetUserProfileByIdAsync(ApplicationUser user, int profileId)
    {
        Logger.LogInformation("Fetching profile: {profileId} for user: {userId}", profileId, user.Id);

        var profile = await DbContext.Profiles
            .Include(profile => profile.Account)
            .FirstOrDefaultAsync(profile => profile.Id == profileId);

        return profile;
    }

    public async Task<IEnumerable<Profile>> GetUserProfilesAsync(ApplicationUser user)
    {
        Logger.LogInformation("Fetching profiles for user: {userId}", user.Id);

        var profile = await DbContext.Profiles
            .Where(profile => profile.Account.Id == user.Id)
            .ToListAsync();

        return profile;
    }

    public async Task<IEnumerable<WatchedMovie>> GetWatchedMoviesAsync(Profile profile)
    {
        Logger.LogInformation("Fetching watched movies for profile: {userId}", profile.Id);

        var watchedMovies = await DbContext.WatchedMovies
            .Where(watchedMovie => watchedMovie.Profile.Id == profile.Id)
            .ToListAsync();

        return watchedMovies;
    }
}