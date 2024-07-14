namespace OpenMovies.WebApi.Services;

public sealed class ProfileManager(
    UserManager<ApplicationUser> userManager,
    IProfileRepository profileRepository,
    IProfileCreationPolicy profileCreationPolicy,
    IAvatarImageProvider avatarImageProvider,
    ILogger<ProfileManager> logger
) : IProfileManager
{
    public async Task<OperationResult> SaveUserProfileAsync(string userId, Profile profile)
    {
        var user = await GetUserByIdAsync(userId);

        if (!await profileCreationPolicy.CanCreateProfileAsync(userId))
            return OperationResult.Failure("Maximum number of profiles reached.");

        if (string.IsNullOrEmpty(profile.Avatar))
            profile.Avatar = avatarImageProvider.GetRandomDefaultAvatar();

        profile.Account = user;

        var result = await profileRepository.SaveAsync(profile);
        if (!result.IsSuccess)
            return result;

        await userManager.UpdateAsync(user);

        logger.LogInformation("Profile saved successfully.");
        return OperationResult.Success("Profile saved successfully.");
    }

    public async Task<OperationResult> DeleteUserProfileAsync(string userId, int profileId)
    {
        var user = await GetUserByIdAsync(userId);

        var profile = user.Profiles.FirstOrDefault(profile => profile.Id == profileId);
        if (profile is null)
            return OperationResult.Failure("Profile not found.");

        var result = await profileRepository.DeleteAsync(profile);
        if (!result.IsSuccess)
            return result;

        await userManager.UpdateAsync(user);

        logger.LogInformation("user profile `{userId}` with ID: `{profile}` successfully deleted.", userId, profileId);
        return OperationResult.Success(message: "Profile deleted successfully.");
    }

    public async Task<OperationResult> UpdateUserProfileAsync(string userId, Profile profile)
    {
        var user = await GetUserByIdAsync(userId);

        var result = await profileRepository.UpdateAsync(profile);
        if (!result.IsSuccess)
            return result;

        await userManager.UpdateAsync(user);

        logger.LogInformation("Profile updated successfully.");
        return OperationResult.Success("Profile updated successfully.");
    }

    public async Task<Profile?> GetUserProfileByIdAsync(string userId, int profileId)
    {
        var user = await GetUserByIdAsync(userId);

        return await profileRepository.GetUserProfileByIdAsync(user, profileId);
    }

    public async Task<IEnumerable<Profile>> GetUserProfilesAsync(string userId)
    {
        var user = await GetUserByIdAsync(userId);

        return await profileRepository.GetUserProfilesAsync(user);
    }

    private async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new UserNotFoundException(userId);

        return user;
    }
}