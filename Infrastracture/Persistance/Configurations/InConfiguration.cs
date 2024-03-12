namespace Infrastructure.Persistance.Configurations;

public class InConfiguration : IEntityTypeConfiguration<In>
{
    public void Configure(EntityTypeBuilder<In> builder)
    {
      builder.Property(x=>x.Id).IsRequired();
        builder.Property(x => x.Price).HasDefaultValue(0);
        builder.Property(x=>x.Date).IsRequired();
        builder.Property(x => x.Reason).IsRequired().HasMaxLength(200);
        builder.Property(x=>x.IsConfirmed).IsRequired();
    }
}
