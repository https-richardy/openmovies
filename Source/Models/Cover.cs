using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMovies.Models;

public class Cover
{
    [NotMapped]
    public IFormFile FileContent { get; set; }

    #pragma warning disable CS8618
    public Cover() {  }
}