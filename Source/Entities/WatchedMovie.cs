namespace OpenMovies.WebApi.Entities;

public sealed class WatchedMovie : Entity
{
    public Movie Movie { get; set; }
    public Profile Profile { get; set; }

    public WatchedMovie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public WatchedMovie(Movie movie, Profile profile)
    {
        Movie = movie;
        Profile = profile;
    }
}