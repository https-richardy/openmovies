namespace OpenMovies.WebApi.Data.Configuration;

public sealed class EpisodeEntityConfiguration : IEntityTypeConfiguration<Episode>
{
    [ExcludeFromCodeCoverage]
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episodes");
        builder.HasKey(episode => episode.Id);

        builder.Property(episode => episode.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(episode => episode.Synopsis)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(episode => episode.VideoSource)
            .IsRequired();

        builder.Property(episode => episode.Number)
            .IsRequired();

        builder.Property(episode => episode.SeasonNumber)
            .IsRequired();
    }
}