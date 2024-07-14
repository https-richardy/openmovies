namespace OpenMovies.WebApi.Entities;

public sealed class Episode : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string VideoSource { get; set; }
    public int Number { get; set; }
    public int SeasonNumber { get; set; }
    public Series Series { get; set; }

    public Episode()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}