namespace Infrastructure.Persistance.Configurations;

internal class SpendConfiguration : IEntityTypeConfiguration<Spend>
{
    public void Configure(EntityTypeBuilder<Spend> builder)
    {
        builder.Property(x=>x.Price).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.IsConfirmed).IsRequired();
    }
}
