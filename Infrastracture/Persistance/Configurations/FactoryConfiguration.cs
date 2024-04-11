
namespace Infrastructure.Persistance.Configurations;

public class FactoryConfiguration : IEntityTypeConfiguration<Factory>
{
    public void Configure(EntityTypeBuilder<Factory> builder)
    {
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
    }
}

