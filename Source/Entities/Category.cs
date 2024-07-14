namespace OpenMovies.WebApi.Entities;

public sealed class Category : Entity
{
    public string Name { get; set; }

    public Category()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public Category(string name)
    {
        Name = name;
    }
}