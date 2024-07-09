namespace OpenMovies.WebApi.Extensions;

public static class PipelineExtension
{
    [ExcludeFromCodeCoverage]
    public static void ConfigureHttpPipeline(this IApplicationBuilder app)
    {
        app.ConfigureCORS();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseValidationExceptionHandler();
    }
}