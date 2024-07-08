namespace OpenMovies.WebApi.Data.Configuration;

public sealed class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        builder.HasKey(movie => movie.Id);

        builder.Property(movie => movie.Title)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(movie => movie.Synopsis)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(movie => movie.ImageUrl)
            .HasMaxLength(700)
            .IsRequired();

        builder.Property(movie => movie.VideoSource)
            .HasMaxLength(700)
            .IsRequired();

        builder.Property(movie => movie.ReleaseYear)
            .IsRequired();

        builder.Property(movie => movie.DurationInMinutes)
            .IsRequired();

        builder.HasOne(movie => movie.Category);
    }
}