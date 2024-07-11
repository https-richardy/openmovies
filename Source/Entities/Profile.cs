namespace OpenMovies.WebApi.Entities;

public sealed class Profile : Entity
{
    public string Name { get; set; }
    public bool IsChild { get; set; }

    public ApplicationUser Account { get; set; }

    public ICollection<BookmarkedMovie> BookmarkedMovies { get; set; }
    public ICollection<WatchedMovie> WatchedMovies { get; set; }

    public Profile()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}