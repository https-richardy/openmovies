namespace OpenMovies.WebApi.Models;

public sealed class Movie : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string ImagePath { get; set; }
    public string VideoSource { get; set; }
    public int ReleaseYear { get; set; }

    public MovieCategory Category { get; set; }
    public TimeSpan Duration { get; set; }

    public Movie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public Movie(string title, string synopsis, string imagePath, string videoSource, int releaseYear, MovieCategory category, TimeSpan duration)
    {
        Title = title;
        Synopsis = synopsis;
        ImagePath = imagePath;
        VideoSource = videoSource;
        ReleaseYear = releaseYear;
        Category = category;
        Duration = duration;
    }
}