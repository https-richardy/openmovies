namespace OpenMovies.WebApi.Middlewares;

public class ObjectDoesNotExistExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ObjectDoesNotExistExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ObjectDoesNotExistException exception)
        {
            httpContext.Response.StatusCode = 404;
            httpContext.Response.ContentType = "text/plain";

            await httpContext.Response.WriteAsync(exception.Message);
        }
    }
}