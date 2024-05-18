using OpenMovies.WebApi.Middlewares;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.ConfigureServices(configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader());
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.UseStaticFiles();

        app.UseValidationExceptionHandler();

        app.Run();
    }
}