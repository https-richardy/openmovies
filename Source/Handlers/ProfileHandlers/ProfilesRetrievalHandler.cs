namespace OpenMovies.WebApi.Handlers;

public sealed class ProfilesRetrievalHandler(
    IUserContextService userContextService,
    IProfileManager profileManager
) : IRequestHandler<ProfilesRetrievalRequest, Response<IEnumerable<ProfileInformation>>>
{
    #pragma warning disable CS8604
    public async Task<Response<IEnumerable<ProfileInformation>>> Handle(
        ProfilesRetrievalRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContextService.GetCurrentUserId();

        var profiles = await profileManager.GetUserProfilesAsync(userId);
        var formattedProfiles = new List<ProfileInformation>();

        foreach (var profile in profiles)
        {
            var formattedProfile = TinyMapper.Map<ProfileInformation>(profile);
            formattedProfiles.Add(formattedProfile);
        }

        return new Response<IEnumerable<ProfileInformation>>(
            data: formattedProfiles,
            statusCode: StatusCodes.Status200OK,
            message: "Profiles successfully recovered."
        );
    }
}
