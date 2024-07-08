namespace OpenMovies.WebApi.Services;

/// <summary>
/// Service responsible for creating JSON Web Tokens (JWT) based on provided user information (claims identity).
/// </summary>
/// <remarks>
/// This service requires a secret key for encoding and decoding JWT, which is typically stored
/// in the application's configuration settings (appsettings.json).
/// </remarks>
public sealed class JwtService : IJwtService
{
    private readonly byte[] _secretKey;
    private JwtOptions _options;

    #pragma warning disable CS8604
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="configuration">Configuration provider that contains the secret key for JWT.</param>
    /// <remarks>
    /// This constructor retrieves the secret key from the application configuration
    /// and initializes the JWT service with the default options.
    /// </remarks>
    public JwtService(IConfiguration configuration)
    {
        _secretKey = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);
        _options = new JwtOptions { Key = _secretKey }; 
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class with custom JWT options.
    /// </summary>
    /// <param name="configuration">Configuration provider that contains the secret key for JWT.</param>
    /// <param name="options">Customized options for configuring the JWT service.</param>
    /// <remarks>
    /// This constructor allows for customization of the JWT service by providing
    /// a <see cref="JwtOptions"/> object with custom settings. If the options do not
    /// include a key, it will be automatically set using the secret key from the configuration (appsettings.json).
    /// </remarks>

    public JwtService(IConfiguration configuration, JwtOptions options)
    : this(configuration)
    {
        _options = options;

        if (options.Key is null)
            _options.Key = _secretKey;
    }

    /// <summary>
    /// Generates a JWT based on the provided claims identity.
    /// </summary>
    /// <param name="claimsIdentity">User information and claims to be included in the JWT</param>
    /// <returns>Generated JWT as a string</returns>
    public string GenerateToken(ClaimsIdentity claimsIdentity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = CreateTokenDescriptor(claimsIdentity);
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = _options.Expires,
            SigningCredentials = _options.SigningCredentials
        };

        return securityTokenDescriptor;
    }
}