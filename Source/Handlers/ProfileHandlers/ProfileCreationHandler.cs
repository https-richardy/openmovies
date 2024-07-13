namespace OpenMovies.WebApi.Handlers;

public sealed class AddNewProfileHandler(
    UserManager<ApplicationUser> userManager,
    IUserContextService userContextService,
    IProfileManager profileManager,
    IFileUploadService fileUploadService
) : IRequestHandler<ProfileCreationRequest, Response>
{
    #pragma warning disable CS8604
    public async Task<Response> Handle(
        ProfileCreationRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = userContextService.GetCurrentUserId();
            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
                return new Response(
                    statusCode: StatusCodes.Status404NotFound,
                    message: "User not found."
                );

            var profile = new Profile
            {
                Name = request.Name,
                Account = user
            };

            if (request.Avatar != null)
            {
                var imagePath = await fileUploadService.UploadFileAsync(request.Avatar);
                profile.Avatar = imagePath;
            }

            await profileManager.SaveUserProfileAsync(user.Id, profile);
            return new Response
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "Profile created successfully."
            };

        }
        catch (MaxProfileCountReachedException exception)
        {
            return new Response
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = exception.Message
            };
        }
    }
}