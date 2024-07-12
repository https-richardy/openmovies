namespace OpenMovies.WebApi.Services;

public interface IProfileService
{
    Task AddProfileAsync(string userId, Profile profile);
    Task UpdateProfileAsync(string userId, Profile profile);
    Task DeleteProfileAsync(string userId, Profile profile);

    Task<Profile?> GetProfileAsync(string userId, int profileId);
    Task<IEnumerable<Profile>> GetUserProfilesAsync(string userId);
    Task<IEnumerable<BookmarkedMovie>> GetFavoriteMoviesAsync(string userId, int profileId);
    Task<IEnumerable<WatchedMovie>> GetWatchHistoryAsync(string userId, int profileId);

    Task FavoriteMovieAsync(Profile profile, Movie movie);
    Task UnfavoriteMovieAsync(Profile profile, Movie movie);
    Task MarkAsWatchedAsync(Profile profile, Movie movie);
}
