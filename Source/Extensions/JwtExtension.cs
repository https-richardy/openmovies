#pragma warning disable CS8604, CS8618

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenMovies.Services;

namespace OpenMovies.WebApi.Extensions;

public static class JwtExtension
{
    public static void AddBearerJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

        services.AddScoped<JwtService>();
        services.AddScoped<AuthService>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),  // warning disable: CS8604
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            });
    }
}