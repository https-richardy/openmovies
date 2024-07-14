namespace OpenMovies.WebApi.Data.Repositories;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetUserProfileByIdAsync(ApplicationUser user, int profileId);
    Task<IEnumerable<Profile>> GetUserProfilesAsync(ApplicationUser user);
}