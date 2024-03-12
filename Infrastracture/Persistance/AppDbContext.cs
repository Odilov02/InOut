using Application.Common.Extentions;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistance;

public class AppDbContext : IdentityDbContext<User, Role, Guid>, IAppDbContext
{
    public DbSet<In> Ins { get; set; }
    public DbSet<Spend> Spends { get; set; }
    public DbSet<AdminSpend> AdminSpends { get; set; }
    public DbSet<Construction> Constructions { get; set; }
    public DbSet<SpendType> SpendTypes { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SpendType>().HasData(new SpendType
        {
            Id=Guid.NewGuid(),
            Name = "У́зимизни ишчилар харажатлари",
            Descraption = "У́зимизни ишчилар харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Озик-овкат харажатлари",
            Descraption = "Озик-овкат харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Иш хаклари",
            Descraption = "Иш хаклари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Транспорт харажатлари",
            Descraption = "Транспорт харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Хужжатлар ва офис харажатлари",
            Descraption = "Хужжатлар ва офис харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Курилиш материаллар харажатлари",
            Descraption = "Курилиш материаллар харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Иш куроллар харажатлари",
            Descraption = "Иш куроллар харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Бошка майда харажатлар",
            Descraption = "Бошка майда харажатлар"
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
            PhoneNumber="+998942922288",
            Password = "DiyorbekOdilov19".stringHash(),
            SecurityStamp = Guid.NewGuid().ToString()
        });;
        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = roleId,
            UserId = adminId
        });
        base.OnModelCreating(builder);
    }
}

