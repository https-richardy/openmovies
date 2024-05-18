namespace OpenMovies.WebApi.Extensions;

public static class MappingExtension
{
    public static void AddMapping(this IServiceCollection services)
    {
        TinyMapper.Bind<MovieCreationRequest, Movie>(config =>
        {
            config.Bind(source => source.Title, target => target.Title);
            config.Bind(source => source.Synopsis, target => target.Synopsis);
            config.Bind(source => source.ReleaseYear, target => target.ReleaseYear);
        });
    }
}