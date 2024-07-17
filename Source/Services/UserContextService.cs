namespace OpenMovies.WebApi.Services;

public sealed class UserContextService(
    IHttpContextAccessor contextAccessor,
    ILogger<UserContextService> logger
) : IUserContextService
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private readonly ILogger<UserContextService> _logger = logger;

    public string? GetCurrentUserId()
    {
        var currentUserId = GetCurrentUserClaimsPrincipal()?.FindFirstValue(ClaimTypes.NameIdentifier);
        return currentUserId;
    }

    public ClaimsPrincipal? GetCurrentUserClaimsPrincipal()
    {
        var claimsPrincipal = _contextAccessor.HttpContext?.User;

        var currentUserId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentName = claimsPrincipal?.FindFirstValue(ClaimTypes.Name);
        var currentRole = claimsPrincipal?.FindFirstValue(ClaimTypes.Role);

        if (claimsPrincipal == null)
            _logger.LogWarning("No user found in the context.");

        _logger.LogInformation(
            "Current user information: Id: {CurrentUserId}, Name: {CurrentName}, Role: {CurrentUserRole}",
            currentUserId, currentName, currentRole
        );

        return _contextAccessor.HttpContext?.User;
    }

    public int GetCurrentActiveProfileId()
    {
        var claimsPrincipal = _contextAccessor.HttpContext?.User;

        var currentName = claimsPrincipal?.FindFirstValue(ClaimTypes.Name);
        var currentActiveProfileId = claimsPrincipal?.FindFirstValue(CustomClaimTypes.ActiveProfileIdentifier);
        var currentActiveProfileName = claimsPrincipal?.FindFirstValue(CustomClaimTypes.ActiveProfileName);

        _logger.LogInformation(
            "Current user information: Name: {CurrentName}, Active Profile Id: {CurrentActiveProfileId}, Active Profile Name: {CurrentActiveProfileName}",
            currentName, currentActiveProfileId, currentActiveProfileName
        );

        return int.TryParse(currentActiveProfileId, out var profileId) ? profileId : 0;
    }
}