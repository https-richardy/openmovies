namespace OpenMovies.WebApi.Extensions;

public static class DataPersistenceExtension
{
    [ExcludeFromCodeCoverage]
    public static void AddDataPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}