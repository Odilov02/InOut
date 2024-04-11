
namespace Infrastructure.Persistance.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.Property(x=>x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ImgUrl).IsRequired();

    }
}
