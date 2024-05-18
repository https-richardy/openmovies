namespace OpenMovies.WebApi.Extensions;

public static class WebApplicationExtension
{
    public static void ConfigureWebApplication(this IServiceCollection services)
    {
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