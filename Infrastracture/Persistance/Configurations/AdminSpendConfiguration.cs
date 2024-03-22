namespace Infrastructure.Persistance.Configurations;

public class AdminSpendConfiguration : IEntityTypeConfiguration<AdminSpend>
{
    public void Configure(EntityTypeBuilder<AdminSpend> builder)
    {
        builder.Property(x=>x.CreatedDate).IsRequired();
        builder.Property(x => x.Reason).IsRequired().HasMaxLength(200);
    }
}
