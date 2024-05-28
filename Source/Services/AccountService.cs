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
            throw new ObjectDoesNotExistException("User does not exist.");

        var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordIsCorrect)
            return AuthenticationResponse.InvalidCredentialsResponse();

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

    public async Task<OperationResult> RegisterUserAsync(AccountRegistrationRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new UserAlreadyExistsException("Email address is already registered.");

        var newUser = TinyMapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            return OperationResult.FailureResponse("Failed to create user.");

        await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, Role.CommonUser));

        return OperationResult.SuccessResponse("User created successfully.");
    }
}