namespace OpenMovies.WebApi.Extensions;

public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AccountRegistrationRequest>, AccountRegistrationValidator>();
        services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();
    }
}