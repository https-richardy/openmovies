namespace OpenMovies.WebApi.Handlers;

public sealed class ProfileDeletionHandler(
    IUserContextService userContextService,
    IProfileManager profileManager
) : IRequestHandler<ProfileDeletionRequest, Response>
{
    #pragma warning disable CS8604
    public async Task<Response> Handle(
        ProfileDeletionRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContextService.GetCurrentUserId();

        var result = await profileManager.DeleteUserProfileAsync(userId, request.ProfileId);
        if (!result.IsSuccess)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: result.Message
            );

        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "Profile deleted successfully."
        );
    }
}