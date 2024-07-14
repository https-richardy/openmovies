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

        #region mapping settings for profiles

        TinyMapper.Bind<ProfileCreationRequest, Profile>(config =>
        {
            config.Bind(target: target => target.Name, source: source => source.Name);
            config.Bind(target: target => target.IsChild, source: source => source.IsChild);
            config.Ignore(target => target.Avatar);
        });

        TinyMapper.Bind<ProfileEditingRequest, Profile>(config =>
        {
            config.Bind(target: target => target.Name, source: source => source.Name);
            config.Bind(target: target => target.IsChild, source: source => source.IsChild);
            config.Ignore(target => target.Avatar);
        });


        TinyMapper.Bind<Profile, ProfileInformation>(config =>
        {
            config.Bind(target: target => target.Id, source: source => source.Id);
            config.Bind(target: target => target.DisplayName, source: source => source.Name);
            config.Bind(target: target => target.Avatar, source: source => source.Avatar);
        });

        #endregion
    }
}