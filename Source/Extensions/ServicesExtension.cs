namespace OpenMovies.WebApi.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataPersistence(configuration);
        services.AddApplicationServices();

        services.ConfigureIdentity();
        services.AddBearerJwt(configuration);
    }
}