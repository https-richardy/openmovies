namespace OpenMovies.WebApi.Extensions;

public static class WebApplicationExtension
{
    [ExcludeFromCodeCoverage]
    public static void ConfigureWebApplication(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                /* 
                    The following line is added to prevent issues related to circular references
                    during JSON serialization. It instructs the JsonSerializer to ignore cycles.
                */
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }
}