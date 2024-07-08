namespace OpenMovies.WebApi.Handlers;

public sealed class AccountRegistrationHandler(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
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

        var user = new IdentityUser
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

        return new Response(
            statusCode: StatusCodes.Status201Created,
            message: "Account registration successful."
        );
    }
}