namespace OpenMovies.WebApi.Extensions;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IAccountService, AccountService>();

        services.AddFileUploadService();
    }
}