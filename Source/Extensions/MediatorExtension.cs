namespace OpenMovies.WebApi.Extensions;

public static class MediatorExtension
{
    [ExcludeFromCodeCoverage]
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        #region handlers for identity

        services.AddScoped<IRequestHandler<AccountRegistrationRequest, Response>, AccountRegistrationHandler>();
        services.AddScoped<IRequestHandler<AuthenticationCredentials, Response<AuthenticationResponse>>, AuthenticationHandler>();

        #endregion

        #region handlers for movies

        services.AddScoped<IRequestHandler<MovieCreationRequest, Response>, MovieCreationHandler>();

        #endregion

        #region handlers for categories

        services.AddScoped<IRequestHandler<CategoryCreationRequest, Response>, CategoryCreationHandler>();

        #endregion
    }
}