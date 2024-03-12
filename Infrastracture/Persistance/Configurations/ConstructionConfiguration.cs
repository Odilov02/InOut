namespace Infrastructure.Persistance.Configurations;

public class ConstructionConfiguration : IEntityTypeConfiguration<Construction>
{
    public void Configure(EntityTypeBuilder<Construction> builder)
    {
        builder.Property(x=>x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x=>x.FullName).IsRequired().HasMaxLength(50);
    }
}
