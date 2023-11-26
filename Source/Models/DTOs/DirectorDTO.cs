using System.ComponentModel.DataAnnotations;

namespace OpenMovies.Models;

public class DirectorDTO
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set;} = string.Empty;
}