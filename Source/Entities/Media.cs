namespace OpenMovies.WebApi.Entities;

public abstract class Media : Entity
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string ImageUrl { get; set; }
    public int ReleaseYear { get; set; }
    public Category Category { get; set; }

    public Media()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}