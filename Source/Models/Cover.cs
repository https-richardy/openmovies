using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMovies.Models;

public class Cover : Entity
{
    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public IFormFile FileContent { get; set; }

    #pragma warning disable CS8618
    public Cover() {  }
}