namespace OpenMovies.WebApi.Middlewares;

public static class ObjectDoesNotExistExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseObjectDoesNotExistExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ObjectDoesNotExistExceptionMiddleware>();
    }
}