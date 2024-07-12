namespace OpenMovies.WebApi.Handlers;

public sealed class AccountRegistrationHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IProfileManager profileManager,
    IValidator<AccountRegistrationRequest> validator
) : IRequestHandler<AccountRegistrationRequest, Response>
{
    public async Task<Response> Handle(
        AccountRegistrationRequest request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = new ApplicationUser
        {
            UserName = request.FullName,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new Response(
                statusCode: StatusCodes.Status400BadRequest,
                message: "Failed to create user."
            );

        if (!await roleManager.RoleExistsAsync("Common"))
            await roleManager.CreateAsync(new IdentityRole("Common"));

        await userManager.AddToRoleAsync(user, "Common");

        var profile = new Profile { Name = request.FullName, Account = user };
        await profileManager.SaveUserProfileAsync(user.Id, profile);

        return new Response(
            statusCode: StatusCodes.Status201Created,
            message: "Account registration successful."
        );
    }
}