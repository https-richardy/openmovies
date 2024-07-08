namespace OpenMovies.WebApi.Entities;

public sealed class Movie : Media
{
    public string VideoSource { get; set; }
    public int DurationInMinutes { get; set; }

    public Movie()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}