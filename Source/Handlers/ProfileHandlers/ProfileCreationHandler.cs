namespace OpenMovies.WebApi.Handlers;

public sealed class ProfileCreationHandler(
    UserManager<ApplicationUser> userManager,
    IUserContextService userContextService,
    IProfileManager profileManager,
    IFileUploadService fileUploadService,
    IValidator<ProfileCreationRequest> validator
) : IRequestHandler<ProfileCreationRequest, Response>
{
    #pragma warning disable CS8604
    public async Task<Response> Handle(
        ProfileCreationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContextService.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "User not found."
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

        var result = await profileManager.SaveUserProfileAsync(user.Id, profile);
        if (!result.IsSuccess)
            return new Response(
                statusCode: StatusCodes.Status403Forbidden,
                message: result.Message
            );

        return new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Profile created successfully."
        };
    }
}