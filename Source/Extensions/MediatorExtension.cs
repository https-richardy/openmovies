namespace OpenMovies.WebApi.Extensions;

public static class MediatorExtension
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        services.AddScoped<IRequestHandler<AccountRegistrationRequest, OperationResult>, AccountRegistrationHandler>();
        services.AddScoped<IRequestHandler<AuthenticationRequest, AuthenticationResponse>, AuthenticationRequestHandler>();

        services.AddScoped<IRequestHandler<MovieCreationRequest, OperationResult>, MovieCreationHandler>();
        services.AddScoped<IRequestHandler<MovieDeletionRequest, OperationResult>, MovieDeletionHandler>();
    }
}