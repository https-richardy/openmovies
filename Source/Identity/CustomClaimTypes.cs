namespace OpenMovies.WebApi.Identity;

/// <summary>
/// Custom claim types for OpenMovies.
/// </summary>
public static class CustomClaimTypes
{
    /// <summary>
    /// The claim type for the active profile identifier.
    /// </summary>
    public const string ActiveProfileIdentifier = "http://schemas.openmovies/openmovies/claimtypes/activeprofileidentifier";

    /// <summary>
    /// The claim type for the active profile name.
    /// </summary>
    public const string ActiveProfileName = "http://schemas.openmovies/openmovies/claimtypes/activeprofilename";
}