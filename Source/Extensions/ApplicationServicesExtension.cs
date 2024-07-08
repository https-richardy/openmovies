namespace OpenMovies.WebApi.Extensions;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddFileUploadService();
    }
}