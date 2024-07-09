namespace OpenMovies.WebApi.Entities;

public sealed class BookmarkedMovie : Entity
{
    public Movie Movie { get; set; }
    public IdentityUser User { get; set; }

    public BookmarkedMovie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public BookmarkedMovie(Movie movie, IdentityUser user)
    {
        Movie = movie;
        User = user;
    }
}