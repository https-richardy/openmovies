namespace OpenMovies.WebApi.Entities;

public sealed class Movie : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public int ReleaseYear { get; set; }
    public string VideoSource { get; set; }
    public int DurationInMinutes { get; set; }
    public Category Category { get; set; }

    public Movie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}