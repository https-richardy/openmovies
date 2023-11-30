using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMovies.Models;

public class Image : Entity
{
    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public IFormFile FileContent { get; set; }

    #pragma warning disable CS8618
    public Image() {  }
}