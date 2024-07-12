namespace OpenMovies.WebApi.Services;

public sealed class ProfileManager(
    UserManager<ApplicationUser> userManager,
    IProfileRepository profileRepository,
    IProfileCreationPolicy profileCreationPolicy,
    IWebHostEnvironment webHostEnvironment,
    ILogger<ProfileManager> logger
) : IProfileManager
{
    public async Task<OperationResult> SaveUserProfileAsync(string userId, Profile profile)
    {
        var user = await GetUserByIdAsync(userId);

        if (!await profileCreationPolicy.CanCreateProfileAsync(userId))
            return OperationResult.Failure("Maximum number of profiles reached.");

        if (string.IsNullOrEmpty(profile.Avatar))
            profile.Avatar = GetRandomDefaultAvatar();

        user.Profiles.Add(profile);

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

        user.Profiles.Remove(profile);

        var result = await profileRepository.DeleteAsync(profile);
        if (!result.IsSuccess)
            return result;

        await userManager.UpdateAsync(user);

        logger.LogInformation("user profile `{userId}` with ID: `{profile}` successfully updated.", userId, profileId);
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
        return user.Profiles.FirstOrDefault(profile => profile.Id == profileId);
    }

    public async Task<IEnumerable<Profile>> GetUserProfilesAsync(string userId)
    {
        var user = await GetUserByIdAsync(userId);
        return user.Profiles;
    }

    private async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new UserNotFoundException(userId);

        return user;
    }

    private string GetRandomDefaultAvatar()
    {
        var avatarFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "assets", "avatars");
        var avatarFilePaths = Directory.GetFiles(avatarFolderPath);

        var random = new Random();

        var randomIndex = random.Next(avatarFilePaths.Length);
        var selectedFilePath = avatarFilePaths[randomIndex];

        var fileName = Path.GetFileName(selectedFilePath);
        var relativePath = Path.Combine("assets", "avatars", fileName);

        return relativePath;
    }
}