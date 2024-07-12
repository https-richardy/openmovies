namespace OpenMovies.WebApi.Extensions;

public static class PolicyExtension
{
    public static void AddApplicationPolicies(this IServiceCollection services)
    {
        services.AddScoped<IProfileCreationPolicy, MaxProfileCountPolicy>();
    }
}