namespace OpenMovies.WebApi.Entities;

/// <summary>
/// Represents the age classification for movies.
/// </summary>
/// <remarks>
/// This enum is used to categorize movies based on their suitability for different age groups.
/// </remarks>
public enum AgeClassification
{
    /// <summary>
    /// Suitable for general audiences.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie is appropriate for all age groups.
    /// </remarks>
    GeneralAudience,

    /// <summary>
    /// Suitable for audiences aged 10 and above.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie may contain content suitable for children 10 years and older.
    /// </remarks>
    Age10,

    /// <summary>
    /// Suitable for audiences aged 12 and above.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie may contain content suitable for children 12 years and older.
    /// </remarks>
    Age12,

    /// <summary>
    /// Suitable for audiences aged 14 and above.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie may contain content suitable for teenagers 14 years and older.
    /// </remarks>
    Age14,

    /// <summary>
    /// Suitable for audiences aged 16 and above.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie may contain content suitable for older teenagers 16 years and older.
    /// </remarks>
    Age16,

    /// <summary>
    /// Suitable for audiences aged 18 and above.
    /// </summary>
    /// <remarks>
    /// This classification indicates that the movie is intended for adult audiences and may contain mature content.
    /// </remarks>
    Age18
}