namespace OpenMovies.WebApi.Policies;

/// <summary>
/// Interface representing a policy for profile creation.
/// </summary>
public interface IProfileCreationPolicy
{
    /// <summary>
    /// Asynchronously checks if a user can create a new profile.
    /// </summary>
    /// <param name="userId">The ID of the user requesting to create a profile.</param>
    /// <returns>A task that resolves to true if the user can create a profile, false otherwise.</returns>
    Task<bool> CanCreateProfileAsync(string userId);
}
