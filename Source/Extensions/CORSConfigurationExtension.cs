namespace OpenMovies.WebApi.Extensions;

public static class CORSConfigurationExtension
{
    [ExcludeFromCodeCoverage]
    public static void ConfigureCORS(this IApplicationBuilder app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    }
}