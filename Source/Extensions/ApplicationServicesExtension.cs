namespace OpenMovies.WebApi.Extensions;

public static class ApplicationServicesExtension
{
    [ExcludeFromCodeCoverage]
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddFileUploadService();

        services.AddScoped<UserContextService>();
        services.AddScoped<IProfileManager, ProfileManager>();
    }
}