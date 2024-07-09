namespace OpenMovies.WebApi.Services;

public sealed class UserContextService(
    IHttpContextAccessor contextAccessor,
    ILogger<UserContextService> logger
)
{
    public string? GetCurrentUserId()
    {
        var currentUserId = GetCurrentUserClaimsPrincipal()?.FindFirstValue(ClaimTypes.NameIdentifier);
        return currentUserId;
    }

    public ClaimsPrincipal? GetCurrentUserClaimsPrincipal()
    {
        var claimsPrincipal = contextAccessor.HttpContext?.User;

        var currentUserId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentName = claimsPrincipal?.FindFirstValue(ClaimTypes.Name);
        var currentRole = claimsPrincipal?.FindFirstValue(ClaimTypes.Role);

        if (claimsPrincipal == null)
            logger.LogWarning("No user found in the context.");

        logger.LogInformation(
            "Current user information: Id: {CurrentUserId}, Name: {CurrentName}, Role: {CurrentUserRole}",
            currentUserId, currentName, currentRole
        );

        return contextAccessor.HttpContext?.User;
    }
}