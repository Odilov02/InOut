using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
    EntityEntry Entry(object entity);
    DbSet<In> Ins { get; }
    DbSet<Spend> Spends { get; }
    DbSet<AdminSpend> AdminSpends { get; }
    DbSet<SpendType> SpendTypes { get; }
    DbSet<Construction> Constructions { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
