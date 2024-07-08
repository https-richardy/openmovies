namespace OpenMovies.WebApi.Services;

/// <summary>
/// Provides eoptionstensions for configuring JWT services.
/// </summary>
/// <remarks>
/// This class contains eoptionstension methods for adding JWT-related services to the
/// dependency injection container, allowing easy integration with authentication and
/// authorization in ASP.NET Core applications.
/// </remarks>
public static class JwtServiceEoptionstensions
{

    /// <summary>
    /// Adds JWT service to the service collection and configures authentication.
    /// </summary>
    /// <param name="services">The service collection to add the JWT service to.</param>
    /// <param name="configuration">The configuration providing access to application settings.</param>

    #pragma warning disable CS8604
    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>();
        var secretKey = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        services.AddAuthorization();
    }

    /// <summary>
    /// Adds JWT service to the service collection with custom options.
    /// </summary>
    /// <param name="services">The service collection to add the JWT service to.</param>
    /// <param name="configuration">The configuration providing access to application settings.</param>
    /// <param name="configureOptions">An action to configure the JWT options.</param>
    public static void AddJwtBearer(this IServiceCollection services, IConfiguration configuration, Action<JwtOptions> configureOptions)
    {
        var options = new JwtOptions();
        var secretKey = Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"]);

        configureOptions(options);

        services.AddScoped<IJwtService, JwtService>(provider => new JwtService(configuration, options));
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });
 
        services.AddAuthorization();
    }
}
#pragma warning restore CS8604