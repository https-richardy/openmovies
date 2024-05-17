using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenMovies.Models;

namespace OpenMovies.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions options)
    : base(options) {  }
}
