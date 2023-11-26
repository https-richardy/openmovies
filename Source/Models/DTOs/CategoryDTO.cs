using System.ComponentModel.DataAnnotations;

namespace OpenMovies.DTOs;


public class CategoryDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
}