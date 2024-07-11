namespace OpenMovies.WebApi.Data;

public sealed class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<BookmarkedMovie> BookmarkedMovies { get; set; }
    public DbSet<WatchedMovie> WatchedMovies { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Series> Series { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
