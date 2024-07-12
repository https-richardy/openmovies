namespace OpenMovies.WebApi.Services;

public interface IProfileManager
{
    Task<OperationResult> SaveUserProfileAsync(string userId, Profile profile);
    Task<OperationResult> UpdateUserProfileAsync(string userId, Profile profile);
    Task<OperationResult> DeleteUserProfileAsync(string userId, int profileId);

    Task<Profile?> GetUserProfileByIdAsync(string userId, int profileId);
    Task<IEnumerable<Profile>> GetUserProfilesAsync(string userId);
}