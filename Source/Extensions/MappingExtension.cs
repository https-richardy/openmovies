namespace OpenMovies.WebApi.Extensions;

public static class MappingExtension
{
    public static void AddMapping(this IServiceCollection services)
    {
        #region mapping settings for movies

        TinyMapper.Bind<MovieCreationRequest, Movie>(config =>
        {
            config.Bind(target: target => target.Title, source: source => source.Title);
            config.Bind(target: target => target.Synopsis, source: source => source.Synopsis);
            config.Bind(target: target => target.VideoSource, source: source => source.VideoSource);
            config.Bind(target: target => target.ReleaseYear, source: source => source.ReleaseYear);
            config.Bind(target: target => target.DurationInMinutes, source: source => source.DurationInMinutes);
        });

        TinyMapper.Bind<MovieUpdateRequest, Movie>(config =>
        {
            config.Bind(target: target => target.Title, source: source => source.Title);
            config.Bind(target: target => target.Synopsis, source: source => source.Synopsis);
            config.Bind(target: target => target.VideoSource, source: source => source.VideoSource);
            config.Bind(target: target => target.ReleaseYear, source: source => source.ReleaseYear);
            config.Bind(target: target => target.DurationInMinutes, source: source => source.DurationInMinutes);
        });

        #endregion
    }
}