namespace OpenMovies.WebApi.Extensions;

public static class ValidationExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        #region validators for identity

        services.AddScoped<IValidator<AccountRegistrationRequest>, AccountRegistrationValidator>();
        services.AddScoped<IValidator<AuthenticationCredentials>, AuthenticationCredentialsValidator>();

        #endregion

        #region validators for profiles

        services.AddScoped<IValidator<ProfileCreationRequest>, ProfileCreationValidator>();
        services.AddScoped<IValidator<ProfileEditingRequest>, ProfileEditingValidator>();

        #endregion

        #region validators for movies

        services.AddScoped<IValidator<MovieCreationRequest>, MovieCreationValidator>();
        services.AddScoped<IValidator<MovieUpdateRequest>, MovieUpdateValidator>();

        #endregion

        #region validators for categories

        services.AddScoped<IValidator<CategoryCreationRequest>, CategoryCreationValidator>();
        services.AddScoped<IValidator<CategoryUpdateRequest>, CategoryUpdateValidator>();

        #endregion
    }
}