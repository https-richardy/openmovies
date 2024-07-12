namespace OpenMovies.WebApi.Policies;

/// <summary>
/// Implements the <see cref="IProfileCreationPolicy"/> interface to enforce a maximum number of profiles per user.
/// </summary>
/// <remarks>
/// This policy checks if a user has reached the maximum allowed number of profiles before allowing them to create a new one.
/// The maximum number of profiles is defined by the private constant <c>_maxNumberOfProfilesPerAccount</c>.
/// </remarks>
public sealed class MaxProfileCountPolicy(UserManager<ApplicationUser> userManager) : IProfileCreationPolicy
{
    private const int _maxNumberOfProfilesPerAccount = 4;

    /// <summary>
    /// Asynchronously checks if a user can create a new profile based on the maximum profile count.
    /// </summary>
    /// <param name="userId">The ID of the user requesting to create a profile.</param>
    /// <returns>
    /// A task that resolves to true if the user can create a profile (i.e., hasn't reached the maximum limit), 
    /// false otherwise.
    /// </returns>
    public async Task<bool> CanCreateProfileAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            throw new MaxProfileCountReachedException(userId, _maxNumberOfProfilesPerAccount);

        return user.Profiles.Count < _maxNumberOfProfilesPerAccount;
    }
}