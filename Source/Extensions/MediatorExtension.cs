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

        services.AddScoped<IRequestHandler<MovieRetrievalRequest, Response<PaginationHelper<Movie>>>, MovieRetrievalHandler>();
        services.AddScoped<IRequestHandler<MovieDetailsRequest, Response<Movie>>, MovieDetailsHandler>();
        services.AddScoped<IRequestHandler<MovieCreationRequest, Response>, MovieCreationHandler>();
        services.AddScoped<IRequestHandler<MovieUpdateRequest, Response>, MovieUpdateHandler>();
        services.AddScoped<IRequestHandler<MovieDeletionRequest, Response>, MovieDeletionHandler>();

        #endregion

        #region handlers for categories

        services.AddScoped<IRequestHandler<CategoryCreationRequest, Response>, CategoryCreationHandler>();
        services.AddScoped<IRequestHandler<CategoryUpdateRequest, Response>, CategoryUpdateHandler>();
        services.AddScoped<IRequestHandler<CategoryDeletionRequest, Response>, CategoryDeletionHandler>();

        #endregion
    }
}