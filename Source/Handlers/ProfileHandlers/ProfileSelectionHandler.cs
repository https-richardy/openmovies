namespace OpenMovies.WebApi.Handlers;

public sealed class ProfileSelectionHandler(
    UserManager<ApplicationUser> userManager,
    IUserContextService userContextService,
    IProfileManager profileManager,
    IJwtService jwtService
) : IRequestHandler<ProfileSelectionRequest, Response<AuthenticationResponse>>
{
    #pragma warning disable CS8604
    public async Task<Response<AuthenticationResponse>> Handle(
        ProfileSelectionRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContextService.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(userId);

        var userProfiles = await profileManager.GetUserProfilesAsync(userId);
        var profile = userProfiles
            .Where(profile => profile.Id == request.ProfileId)
            .FirstOrDefault();

        if (profile is null)
            return new Response<AuthenticationResponse>(
                data: null,
                statusCode: StatusCodes.Status404NotFound,
                message: "Profile not found."
            );

        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),

            new Claim(CustomClaimTypes.ActiveProfileIdentifier, profile.Id.ToString()),
            new Claim(CustomClaimTypes.ActiveProfileName, profile.Name)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity(claims);
        var token = jwtService.GenerateToken(claimsIdentity);

        return new Response<AuthenticationResponse>(
            data: new AuthenticationResponse { Token = token },
            statusCode: StatusCodes.Status200OK,
            message: "Profile successfully selected."
        );
    }
}