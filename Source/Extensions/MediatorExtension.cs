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

        services.AddScoped<IRequestHandler<CategoryRetrievalRequest, Response<Category>>, CategoryRetrievalHandler>();
        services.AddScoped<IRequestHandler<GetCategoriesRequest, Response<IEnumerable<Category>>>, GetCategoriesHandler>();
        services.AddScoped<IRequestHandler<CategoryCreationRequest, Response>, CategoryCreationHandler>();
        services.AddScoped<IRequestHandler<CategoryUpdateRequest, Response>, CategoryUpdateHandler>();
        services.AddScoped<IRequestHandler<CategoryDeletionRequest, Response>, CategoryDeletionHandler>();

        #endregion

        #region handlers for profiles

        services.AddScoped<IRequestHandler<ProfilesRetrievalRequest, Response<IEnumerable<ProfileInformation>>>, ProfilesRetrievalHandler>();
        services.AddScoped<IRequestHandler<ProfileCreationRequest, Response>, ProfileCreationHandler>();
        services.AddScoped<IRequestHandler<ProfileEditingRequest, Response>, ProfileEditingHandler>();
        services.AddScoped<IRequestHandler<ProfileDeletionRequest, Response>, ProfileDeletionHandler>();
        services.AddScoped<IRequestHandler<ProfileSelectionRequest, Response<AuthenticationResponse>>, ProfileSelectionHandler>();

        #endregion
    }
}