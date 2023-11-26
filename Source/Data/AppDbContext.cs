using Microsoft.EntityFrameworkCore;
using OpenMovies.Models;

namespace OpenMovies.Data;

public class AppDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Trailer> Trailers { get; set; }

    public AppDbContext(DbContextOptions options)
    : base(options) {  }
}
