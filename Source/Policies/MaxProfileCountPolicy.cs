namespace OpenMovies.WebApi.Policies;

public sealed class MaxProfileCountPolicy(UserManager<ApplicationUser> userManager) : IProfileCreationPolicy
{
    private const int _maxNumberOfProfilesPerAccount = 4;

    public async Task<bool> CanCreateProfileAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception(""); // TODO: throw custom exception.

        return user.Profiles.Count < _maxNumberOfProfilesPerAccount;
    }
}