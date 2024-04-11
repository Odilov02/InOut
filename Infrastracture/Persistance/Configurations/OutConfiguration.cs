using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Configurations;

internal class OutConfiguration : IEntityTypeConfiguration<Out>
{
    public void Configure(EntityTypeBuilder<Out> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Reason).IsRequired().HasMaxLength(200);
    }
}