﻿// <auto-generated />
using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistance.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Construction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ContractPrice")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("In")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Spend")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SpendDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Constructions");
                });

            modelBuilder.Entity("Domain.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Domain.Entities.Factory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("In")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Spend")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SpendDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Factories");
                });

            modelBuilder.Entity("Domain.Entities.In", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ConstructionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FactoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionId");

                    b.HasIndex("FactoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Ins");
                });

            modelBuilder.Entity("Domain.Entities.Out", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConstructionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionId");

                    b.ToTable("Outs");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("503e17d3-0151-4e2b-931f-4dff686f6eb3"),
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = new Guid("0ba18909-ca5e-45a7-b39f-2639fe3e74a2"),
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("d9e42cf2-17bc-4aae-b295-6f70a76d9212"),
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Spend", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ConstructionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FactoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCash")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SpendTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionId");

                    b.HasIndex("FactoryId");

                    b.HasIndex("SpendTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Spends");
                });

            modelBuilder.Entity("Domain.Entities.SpendType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descraption")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SpendTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("92366285-fb88-424b-a143-0fb44e44b5dc"),
                            Descraption = "У́зимизни ишчилар харажатлари",
                            Name = "У́зимизни ишчилар"
                        },
                        new
                        {
                            Id = new Guid("867a89b9-1900-471e-9bf4-5eca8796272b"),
                            Descraption = "Озик-овкат харажатлари",
                            Name = "Озик-овкат"
                        },
                        new
                        {
                            Id = new Guid("3b7c5c49-f6fc-4421-9769-7955a3581a52"),
                            Descraption = "Иш хақлари",
                            Name = "Иш хақлари"
                        },
                        new
                        {
                            Id = new Guid("e11b3aca-4de8-48db-aada-e6ebcdd031c5"),
                            Descraption = "Транспорт харажатлари",
                            Name = "Транспорт"
                        },
                        new
                        {
                            Id = new Guid("4b418010-a902-45d6-bdf6-0124cec4fa28"),
                            Descraption = "Хужжатлар ва офис харажатлари",
                            Name = "Хужжатлар ва офис"
                        },
                        new
                        {
                            Id = new Guid("5ee2126e-498d-421f-897e-ca5278ab8881"),
                            Descraption = "Қурилиш материаллар харажатлари",
                            Name = "Қурилиш материаллар"
                        },
                        new
                        {
                            Id = new Guid("ba354941-33ca-4b35-8832-bac3e510b96d"),
                            Descraption = "Иш қуроллар харажатлари",
                            Name = "Иш қуроллар"
                        },
                        new
                        {
                            Id = new Guid("0482f5bd-e81b-4b29-a359-7e6c8d46a1a8"),
                            Descraption = "Бошка майда харажатлар",
                            Name = "Бошқа майда харажатлар"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<long>("Residual")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3343f9f1-0a21-4117-b193-1e53731f58df"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "aac3ddec-65a9-488a-bae1-a671923ec872",
                            EmailConfirmed = false,
                            FullName = "Admin",
                            LockoutEnabled = false,
                            Login = "Admin111",
                            Password = "5b94a8e5341dff953d9847b049d2cebf27f3e96035e9608ea634df0b7a95d2c4",
                            PhoneNumber = "+998912345678",
                            PhoneNumberConfirmed = false,
                            Residual = 0L,
                            SecurityStamp = "03704632-b9f1-4ff3-9187-274a35f5d76b",
                            TwoFactorEnabled = false,
                            UserName = "6025440e-5fbe-458e-8074-f1e408aaa776"
                        },
                        new
                        {
                            Id = new Guid("878962d1-4a08-492e-a9e2-87f0265917a0"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2c8855d2-46a2-4033-b222-1abe257e2dc0",
                            EmailConfirmed = false,
                            FullName = "Admin",
                            LockoutEnabled = false,
                            Login = "Admin000",
                            Password = "628d9f9579184c682f76a9cf717c09050a6f97211477c948c6e027f0da26baa1",
                            PhoneNumber = "+998901234567",
                            PhoneNumberConfirmed = false,
                            Residual = 0L,
                            SecurityStamp = "b9901369-fdf8-41a1-ad06-23b3d22a2ab6",
                            TwoFactorEnabled = false,
                            UserName = "07f1a493-03de-4c2e-b474-b10af9c2f64f"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("3343f9f1-0a21-4117-b193-1e53731f58df"),
                            RoleId = new Guid("0ba18909-ca5e-45a7-b39f-2639fe3e74a2")
                        },
                        new
                        {
                            UserId = new Guid("878962d1-4a08-492e-a9e2-87f0265917a0"),
                            RoleId = new Guid("503e17d3-0151-4e2b-931f-4dff686f6eb3")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Construction", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne("Construction")
                        .HasForeignKey("Domain.Entities.Construction", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.In", b =>
                {
                    b.HasOne("Domain.Entities.Construction", "Construction")
                        .WithMany("Ins")
                        .HasForeignKey("ConstructionId");

                    b.HasOne("Domain.Entities.Factory", "Factory")
                        .WithMany("Ins")
                        .HasForeignKey("FactoryId");

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Ins")
                        .HasForeignKey("UserId");

                    b.Navigation("Construction");

                    b.Navigation("Factory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Out", b =>
                {
                    b.HasOne("Domain.Entities.Construction", "Construction")
                        .WithMany("Outs")
                        .HasForeignKey("ConstructionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Construction");
                });

            modelBuilder.Entity("Domain.Entities.Spend", b =>
                {
                    b.HasOne("Domain.Entities.Construction", "Construction")
                        .WithMany("Spends")
                        .HasForeignKey("ConstructionId");

                    b.HasOne("Domain.Entities.Factory", "Factory")
                        .WithMany("Spends")
                        .HasForeignKey("FactoryId");

                    b.HasOne("Domain.Entities.SpendType", "SpendType")
                        .WithMany("Spends")
                        .HasForeignKey("SpendTypeId");

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Spends")
                        .HasForeignKey("UserId");

                    b.Navigation("Construction");

                    b.Navigation("Factory");

                    b.Navigation("SpendType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Construction", b =>
                {
                    b.Navigation("Ins");

                    b.Navigation("Outs");

                    b.Navigation("Spends");
                });

            modelBuilder.Entity("Domain.Entities.Factory", b =>
                {
                    b.Navigation("Ins");

                    b.Navigation("Spends");
                });

            modelBuilder.Entity("Domain.Entities.SpendType", b =>
                {
                    b.Navigation("Spends");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Construction");

                    b.Navigation("Ins");

                    b.Navigation("Spends");
                });
#pragma warning restore 612, 618
        }
    }
}
