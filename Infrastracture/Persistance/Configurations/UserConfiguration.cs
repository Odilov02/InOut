
namespace Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Residual).HasDefaultValue(0);
        builder.Property(x => x.Login).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Login).IsUnique();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
    }
}
