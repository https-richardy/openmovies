namespace OpenMovies.WebApi.Handlers;

public sealed class AuthenticationHandler(
    UserManager<IdentityUser> userManager,
    IValidator<AuthenticationCredentials> validator,
    IJwtService jwtService
) :
    IRequestHandler<AuthenticationCredentials, Response<AuthenticationResponse>>
{
    #pragma warning disable CS8604
    public async Task<Response<AuthenticationResponse>> Handle(
        AuthenticationCredentials request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new Response<AuthenticationResponse>(
                data: null,
                statusCode: StatusCodes.Status401Unauthorized,
                message: "Invalid email or password."
            );

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            return new Response<AuthenticationResponse>(
                data: null,
                statusCode: StatusCodes.Status401Unauthorized,
                message: "Invalid email or password."
            );

        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
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
            message: "Authentication successful."
        );
    }
}