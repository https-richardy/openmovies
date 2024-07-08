namespace OpenMovies.WebApi.Extensions;

public static class DataPersistenceExtension
{
    public static void AddDataPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
    }
}