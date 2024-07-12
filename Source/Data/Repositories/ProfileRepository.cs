namespace OpenMovies.WebApi.Data.Repositories;

public sealed class ProfileRepository(
    AppDbContext dbContext,
    ILogger<ProfileRepository> logger
) : Repository<Profile, AppDbContext>(dbContext, logger), IProfileRepository
{
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
}