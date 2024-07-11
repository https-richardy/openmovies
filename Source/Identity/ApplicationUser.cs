namespace OpenMovies.WebApi.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public ICollection<Profile> Profiles { get; set; }

    public ApplicationUser()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }
}