namespace OpenMovies.WebApi.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureWebApplication();

        services.AddDataPersistence(configuration);
        services.AddApplicationServices();

        services.ConfigureIdentity();
        services.AddJwtBearer(configuration);

        services.AddMediator();

        services.AddMapping();
        services.AddValidation();
    }
}