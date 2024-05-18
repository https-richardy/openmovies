namespace OpenMovies.WebApi.Services;

public sealed class AccountService : IAccountService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountService(IJwtService jwtService, UserManager<IdentityUser> userManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
    }

    # pragma warning disable CS8604
    public async Task<AuthenticationResponse> AuthenticateUserAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return AuthenticationResponse.FailureResponse("User does not exist.");

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordIsCorrect)
            return AuthenticationResponse.FailureResponse();

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user?.Email),
            new Claim(ClaimTypes.Email, user.Email)
        });

        var token = _jwtService.GenerateToken(claimsIdentity);
        var successfulAuthenticationResponse = AuthenticationResponse.SuccessResponse(token);

        return successfulAuthenticationResponse;
    }

    public async Task<AccountRegistrationResponse> RegisterUserAsync(AccountRegistrationRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            return AccountRegistrationResponse.FailureResponse("Email address is already registered.");

        var newUser = TinyMapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            return AccountRegistrationResponse.FailureResponse("Failed to create user.");

        await _userManager.AddToRoleAsync(newUser, Role.CommonUser);

        return AccountRegistrationResponse.SuccessResponse();
    }
}