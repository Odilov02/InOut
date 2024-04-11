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
    public DbSet<Factory> Factories { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Construction> Constructions { get; set; }
    public DbSet<SpendType> SpendTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Out> Outs { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SpendType>().HasData(new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "У́зимизни ишчилар",
            Descraption = "У́зимизни ишчилар харажатлари"
        },

        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Озик-овкат",
            Descraption = "Озик-овкат харажатлари"
        },

        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Иш хақлари",
            Descraption = "Иш хақлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Транспорт",
            Descraption = "Транспорт харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Хужжатлар ва офис",
            Descraption = "Хужжатлар ва офис харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Қурилиш материаллар",
            Descraption = "Қурилиш материаллар харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Иш қуроллар",
            Descraption = "Иш қуроллар харажатлари"
        },
        new SpendType
        {
            Id = Guid.NewGuid(),
            Name = "Бошқа майда харажатлар",
            Descraption = "Бошка майда харажатлар"
        });

        Guid adminId = Guid.NewGuid();
        Guid adminRoleId = Guid.NewGuid();
        Guid superAdminId = Guid.NewGuid();
        Guid superAdminRoleId = Guid.NewGuid();

        builder.Entity<Role>().HasData(new Role
        {
            Name = "SuperAdmin",
            NormalizedName = "SUPERADMIN",
            Id = superAdminRoleId,
        });

        builder.Entity<Role>().HasData(new Role
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Id = adminRoleId,
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
            Login = "DiyorbekOdilov19",
            PhoneNumber = "+998942922288",
            UserName = Guid.NewGuid().ToString(),
            Password = "DiyorbekOdilov19".stringHash(),
            SecurityStamp = Guid.NewGuid().ToString(),
            Residual = 0
        });

        builder.Entity<User>().HasData(new User
        {
            Id = superAdminId,
            FullName = "Diyorbek Odilov",
            Login = "DiyorbekOdilov20",
            PhoneNumber = "+998942922282",
            UserName = Guid.NewGuid().ToString(),
            Password = "DiyorbekOdilov20".stringHash(),
            SecurityStamp = Guid.NewGuid().ToString(),
            Residual = 0
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = adminRoleId,
            UserId = adminId
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = superAdminRoleId,
            UserId = superAdminId
        });
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}

