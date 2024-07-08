namespace OpenMovies.WebApi.Data.Configuration;

public sealed class SeriesEntityConfiguration : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.ToTable("Series");
        builder.HasKey(series => series.Id);

        builder.Property(series => series.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(series => series.Synopsis)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(series => series.ImageUrl)
            .HasMaxLength(700)
            .IsRequired();

        builder.Property(series => series.ReleaseYear)
            .IsRequired();
    }
}