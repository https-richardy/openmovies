namespace OpenMovies.WebApi.Extensions;

public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        #region validators for identity

        services.AddScoped<IValidator<AccountRegistrationRequest>, AccountRegistrationValidator>();
        services.AddScoped<IValidator<AuthenticationCredentials>, AuthenticationCredentialsValidator>();

        #endregion
    }
}