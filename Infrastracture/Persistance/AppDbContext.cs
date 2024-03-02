﻿using Application.Common.Extentions;
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
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        Guid adminId = Guid.NewGuid();
        Guid roleId = Guid.NewGuid();

        builder.Entity<Role>().HasData(new Role
        {
            Name = "SuperAdmin",
            NormalizedName = "SuperAdmin",
            Id = roleId,
        });

        builder.Entity<Role>().HasData(new Role
        {
            Name = "Admin",
            NormalizedName = "Admin",
            Id = Guid.NewGuid()
        });

        builder.Entity<Role>().HasData(new Role
        {
            Name = "User",
            NormalizedName = "User",
            Id = Guid.NewGuid()
        });

        builder.Entity<User>().HasData(new User
        {
            Id = adminId,
            FullName = "Diyorbek Odilov",
            UserName = "DiyorbekOdilov19",
            Password = "DiyorbekOdilov19".stringHash(),
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
        {
            RoleId = roleId,
            UserId = adminId
        });
    }
}

