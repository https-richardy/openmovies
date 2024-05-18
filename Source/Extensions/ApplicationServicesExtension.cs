namespace OpenMovies.WebApi.Extensions;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddFileUploadService(options =>
        {
            options.AllowedExtensions = new string[] { ".jpeg", ".jpg", ".png"};
        });
    }
}