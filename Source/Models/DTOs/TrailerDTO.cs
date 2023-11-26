using System.ComponentModel.DataAnnotations;
using OpenMovies.Models.Enums;

namespace OpenMovies.Models;

public class TrailerDTO
{
    [Required]
    public TrailerType Type { get; set; }

    [Required]
    public TrailerPlataform Plataform { get; set; }

    [Required]
    public string Link { get; set; } = string.Empty;
}
