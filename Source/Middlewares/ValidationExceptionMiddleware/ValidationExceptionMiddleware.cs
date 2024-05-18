namespace OpenMovies.WebApi.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException validationException)
        {
            httpContext.Response.StatusCode = 400;
            httpContext.Response.ContentType = "application/json";

            var errors = validationException.Errors.Select(error => new
            {
                PropertyName = error.PropertyName,
                ErrorMessage = error.ErrorMessage
            });

            var jsonResponse = JsonSerializer.Serialize(new { Errors = errors });
            await httpContext.Response.WriteAsync(jsonResponse);

            return;
        }
    }
}