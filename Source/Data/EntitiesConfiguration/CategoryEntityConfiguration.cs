namespace OpenMovies.WebApi.Data.Configuration;

public sealed class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    [ExcludeFromCodeCoverage]
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}