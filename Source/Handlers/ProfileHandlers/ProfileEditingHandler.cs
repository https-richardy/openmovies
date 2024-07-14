namespace OpenMovies.WebApi.Handlers;

public sealed class ProfileEditingHandler(
    UserManager<ApplicationUser> userManager,
    IUserContextService userContextService,
    IProfileManager profileManager,
    IFileUploadService fileUploadService,
    IValidator<ProfileEditingRequest> validator
) : IRequestHandler<ProfileEditingRequest, Response>
{
    #pragma warning disable CS8604, CS8602
    public async Task<Response> Handle(
        ProfileEditingRequest request,
        CancellationToken cancellationToken
    )
    {
        var userId = userContextService.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(userId);

        var existingProfile = await profileManager.GetUserProfileByIdAsync(user.Id, request.ProfileId);
        if (existingProfile is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: $"Profile with ID: '{request.ProfileId}' not found."
            );

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var profile = TinyMapper.Map<Profile>(request);

        if (request.Avatar != null)
        {
            var imagePath = await fileUploadService.UploadFileAsync(request.Avatar);
            profile.Avatar = imagePath;
        }
        else
        {
            profile.Avatar = existingProfile.Avatar;
        }

        profile.Id = request.ProfileId;

        await profileManager.UpdateUserProfileAsync(user.Id, profile);
        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "Profile updated successfully."
        );
    }
}