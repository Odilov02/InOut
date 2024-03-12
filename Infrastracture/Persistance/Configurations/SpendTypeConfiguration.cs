namespace Infrastructure.Persistance.Configurations;

public class SpendTypeConfiguration : IEntityTypeConfiguration<SpendType>
{
    public void Configure(EntityTypeBuilder<SpendType> builder)
    {
        builder.Property(x=>x.Descraption).IsRequired().HasMaxLength(200);
    }
}
