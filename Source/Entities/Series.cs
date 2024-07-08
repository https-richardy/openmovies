namespace OpenMovies.WebApi.Entities;

public sealed class Series : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public int ReleaseYear { get; set; }
    public int Seasons { get; set; }

    public Category Category { get; set; }
    public ICollection<Episode> Episodes { get; set; }

    public Series()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}