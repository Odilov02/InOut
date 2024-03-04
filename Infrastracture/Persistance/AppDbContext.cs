using Application.Common.Extentions;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistance;

public class AppDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
{
    public DbSet<In> Ins { get; set; }
    public DbSet<Out> Outs { get; set; }
    public DbSet<Construction> Constructions { get; set; }
    public DbSet<OutType> OutTypes { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<OutType>().HasData(new OutType
        {
            Name = "O'zimizni ishchilar xarajatlari",
            Descraption = "O'zimizni ishchilar xarajatlari"
        },
        new OutType
        {
            Name = "Oziq ovqat",
            Descraption = "Oziq ovqat"
        },
        new OutType
        {
            Name = "Ish xaqlari",
            Descraption = "Ish xaqlari"
        },
        new OutType
        {
            Name = "Transport boyicha",
            Descraption = "Transport boyicha"
        },
        new OutType
        {
            Name = "Hujjatlar va ofis boyicha",
            Descraption = "Hujjatlar va ofis boyicha"
        },
        new OutType
        {
            Name = "Qurilish materiallari",
            Descraption = "Qurilish materiallari"
        },
        new OutType
        {
            Name = "Ish qurollari",
            Descraption = "Ish qurollari"
        },
        new OutType
        {
            Name = "Boshqa mayda xarajatlar",
            Descraption = "Boshqa mayda xarajatlar"
        });

        builder.Entity<OutType>().HasData(new OutType
        {
            Name = "",
            Descraption = ""
        });


        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        Guid adminId = Guid.NewGuid();
        Guid roleId = Guid.NewGuid();

        builder.Entity<Role>().HasData(new Role
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Id = Guid.NewGuid(),
        });

        builder.Entity<Role>().HasData(new Role
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Id = roleId,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        });
        builder.Entity<Role>().HasData(new Role
        {
            Name = "User",
            NormalizedName = "USER",
            Id = Guid.NewGuid()
        });

        builder.Entity<User>().HasData(new User
        {
            Id = adminId,
            FullName = "Diyorbek Odilov",
            UserName = "DiyorbekOdilov19",
            Password = "DiyorbekOdilov19".stringHash(),
            SecurityStamp = Guid.NewGuid().ToString()
        });
        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = roleId,
            UserId = adminId
        });
        base.OnModelCreating(builder);
    }
}

