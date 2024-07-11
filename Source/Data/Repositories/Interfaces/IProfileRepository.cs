namespace OpenMovies.WebApi.Data.Repositories;

public interface IProfileRepository : IRepository<Profile>
{
    Task<IEnumerable<Profile>> GetUserProfilesAsync(ApplicationUser user);
    Task<Profile?> GetUserProfileByIdAsync(ApplicationUser user, int profileId);

    Task<IEnumerable<BookmarkedMovie>> GetBookmarkedMoviesAsync(Profile profile);
    Task<IEnumerable<WatchedMovie>> GetWatchedMoviesAsync(Profile profile);
}