using Microsoft.AspNetCore.Identity;
using OpenMovies.DTOs;

namespace OpenMovies.Services;

public class AuthService
{
    private readonly JwtService _jwtService;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _jwtService = new JwtService(configuration, _userManager);
    }

    public async Task<AuthenticateResponse> GenerateTokenResponseAsync(IdentityUser user)
    {
        var accessToken = await _jwtService.GenerateTokenAsync(user);

        return new AuthenticateResponse
        {
            Token = accessToken,
        };
    }
}