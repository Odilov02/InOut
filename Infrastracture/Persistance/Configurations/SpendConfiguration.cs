namespace Infrastructure.Persistance.Configurations;

internal class SpendConfiguration : IEntityTypeConfiguration<Spend>
{
    public void Configure(EntityTypeBuilder<Spend> builder)
    {
        builder.Property(x => x.IsConfirmed).IsRequired();
    }
}
