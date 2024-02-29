using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    EntityEntry Entry(object entity);
    DbSet<In> Ins { get; }
    DbSet<Out> Outs { get; }
    DbSet<OutType> OutTypes { get; }
    DbSet<Construction> Constructions { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
