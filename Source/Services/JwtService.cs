#pragma warning disable CS8618, CS8601, CS8603, CS8604

namespace OpenMovies.WebApi.Services;

public class JwtService
{
    private readonly byte[] _secretKey;
    private readonly UserManager<IdentityUser> _userManager;

    public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _secretKey = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = CreateTokenDescriptor(user);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }

    private SecurityTokenDescriptor CreateTokenDescriptor(IdentityUser user)
    {
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        });

        var roles = _userManager.GetClaimsAsync(user).Result;
        claims.AddClaims(roles);

        return new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
        };
    }
}