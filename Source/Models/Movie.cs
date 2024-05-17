namespace OpenMovies.WebApi.Models;

public sealed class Movie : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string ImagePath { get; set; }
    public int ReleaseYear { get; set; }

    public Category Category { get; set; }

    public Movie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public Movie(string title, string synopsis, string imagePath, int releaseYear, Category category)
    {
        Title = title;
        Synopsis = synopsis;
        ImagePath = imagePath;
        ReleaseYear = releaseYear;
        Category = category;
    }
}