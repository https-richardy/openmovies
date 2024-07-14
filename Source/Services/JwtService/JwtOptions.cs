namespace OpenMovies.WebApi.Services;

/// <summary>
/// Represents the options for configuring the JSON Web Token (JWT) service.
/// </summary>
/// <remarks>
/// The <see cref="JwtOptions"/> class provides configurable options for generating JWTs.
/// It includes properties for the cryptographic key, security algorithm, and expiration time of the JWT.
/// </remarks>
public record JwtOptions
{
    /// <summary>
    /// Gets or sets the cryptographic key used for encoding and decoding JWT.
    /// </summary>
    /// <remarks>
    /// The key is used to create the <see cref="Microsoft.IdentityModel.Tokens.SymmetricSecurityKey"/> for <see cref="Microsoft.IdentityModel.Tokens.SigningCredentials"/>
    /// </remarks>
    public byte[] Key { get; set; }

    /// <summary>
    /// Gets or sets the security algorithm used for encoding JWT.
    /// </summary>
    /// <remarks>
    /// The default value is <see cref="SecurityAlgorithms.HmacSha256Signature"/>.
    /// </remarks>
    public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

    /// <summary>
    /// Gets or sets the expiration time of the JWT.
    /// </summary>
    /// <remarks>
    /// The default value is the current UTC date and time plus 10 days.
    /// </remarks>
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(10);

    /// <summary>
    /// Gets the <see cref="JwtOptions.SymmetricSecurityKey"/> created from the <see cref="Key"/>
    /// </summary>
    public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Key);

    /// <summary>
    /// Gets the <see cref="SigningCredentials"/> created from the <see cref="SymmetricSecurityKey"/>
    /// and <see cref="SecurityAlgorithm"/>.
    /// </summary>
    public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithm);
}