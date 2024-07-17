namespace OpenMovies.WebApi.Services;

/// <summary>
/// Defines a service to obtain information about the currently authenticated user in the context of the HTTP request.
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Gets the ID of the currently authenticated user.
    /// </summary>
    /// <returns>The user ID, or null if the user is not authenticated.</returns>
    string? GetCurrentUserId();

    int GetCurrentActiveProfileId();

    /// <summary>
    /// Gets the ClaimsPrincipal object of the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// The ClaimsPrincipal contains detailed information about the user's authentication, 
    /// including their claims.
    /// </remarks>
    /// <returns>The user's ClaimsPrincipal object, or null if the user is not authenticated.</returns>
    ClaimsPrincipal? GetCurrentUserClaimsPrincipal();
}