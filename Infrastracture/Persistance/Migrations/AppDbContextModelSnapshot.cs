﻿// <auto-generated />
using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Construction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("ContractPrice")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("In")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Spend")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SpendDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Constructions");
                });

            modelBuilder.Entity("Domain.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("Domain.Entities.Factory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<long>("In")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastedBy")
                        .HasColumnType("text");

                    b.Property<long>("Spend")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SpendDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Factories");
                });

            modelBuilder.Entity("Domain.Entities.In", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ConstructionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("FactoryId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCash")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastedBy")
                        .HasColumnType("text");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

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
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConstructionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionId");

                    b.ToTable("Outs");
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("f169c95b-c9e4-4ab8-b03d-8884968dd711"),
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = new Guid("82562461-6245-4f80-a02c-6a26459b6365"),
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = new Guid("126eb452-60bf-4533-bb69-f33057d4595d"),
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Spend", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<Guid?>("ConstructionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("FactoryId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCash")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Lasted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastedBy")
                        .HasColumnType("text");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<Guid?>("SpendTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

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
                        .HasColumnType("uuid");

                    b.Property<string>("Descraption")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SpendTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4f48c3f2-bd21-4849-8750-2a887399100d"),
                            Descraption = "У́зимизни ишчилар харажатлари",
                            Name = "У́зимизни ишчилар"
                        },
                        new
                        {
                            Id = new Guid("16a8f390-9818-4ccc-a618-6febf295c8b4"),
                            Descraption = "Озик-овкат харажатлари",
                            Name = "Озик-овкат"
                        },
                        new
                        {
                            Id = new Guid("65962610-40e4-4df8-a1c7-6d1d6eaed95a"),
                            Descraption = "Иш хақлари",
                            Name = "Иш хақлари"
                        },
                        new
                        {
                            Id = new Guid("1cd95651-0f58-420a-abaf-77a8fff41706"),
                            Descraption = "Транспорт харажатлари",
                            Name = "Транспорт"
                        },
                        new
                        {
                            Id = new Guid("73fdb1ec-7bd0-42b0-9b9d-3936074e6364"),
                            Descraption = "Хужжатлар ва офис харажатлари",
                            Name = "Хужжатлар ва офис"
                        },
                        new
                        {
                            Id = new Guid("d9139376-f08a-437a-9753-bde8a5cd6f97"),
                            Descraption = "Қурилиш материаллар харажатлари",
                            Name = "Қурилиш материаллар"
                        },
                        new
                        {
                            Id = new Guid("f1bddb54-4239-4163-8755-73d75a617eda"),
                            Descraption = "Иш қуроллар харажатлари",
                            Name = "Иш қуроллар"
                        },
                        new
                        {
                            Id = new Guid("d1c5a046-d3ce-4440-b07a-4fbd19e438ba"),
                            Descraption = "Бошка майда харажатлар",
                            Name = "Бошқа майда харажатлар"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<long>("Residual")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3e67fe66-e253-4285-a1d8-2c5d9a4a8b2c"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "55247d77-2101-46d7-a783-0af4176173e2",
                            EmailConfirmed = false,
                            FullName = "Admin",
                            LockoutEnabled = false,
                            Login = "Admin111",
                            Password = "5b94a8e5341dff953d9847b049d2cebf27f3e96035e9608ea634df0b7a95d2c4",
                            PhoneNumber = "+998912345678",
                            PhoneNumberConfirmed = false,
                            Residual = 0L,
                            SecurityStamp = "67e9fc3c-e0b4-4aab-9a58-4cd50a9dd97f",
                            TwoFactorEnabled = false,
                            UserName = "bc2291f0-2db5-4147-b7e9-8a03b12ec1fe"
                        },
                        new
                        {
                            Id = new Guid("c5ede616-f70f-43c4-a02a-fb2f0b396713"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f97a4f1c-ad66-43a2-b745-72702a2463cf",
                            EmailConfirmed = false,
                            FullName = "Admin",
                            LockoutEnabled = false,
                            Login = "Admin000",
                            Password = "628d9f9579184c682f76a9cf717c09050a6f97211477c948c6e027f0da26baa1",
                            PhoneNumber = "+998901234567",
                            PhoneNumberConfirmed = false,
                            Residual = 0L,
                            SecurityStamp = "32afef20-7c4b-436b-9d74-b7f9e85923e9",
                            TwoFactorEnabled = false,
                            UserName = "381ca63e-44c1-41fc-8cd7-d34842d60867"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("3e67fe66-e253-4285-a1d8-2c5d9a4a8b2c"),
                            RoleId = new Guid("82562461-6245-4f80-a02c-6a26459b6365")
                        },
                        new
                        {
                            UserId = new Guid("c5ede616-f70f-43c4-a02a-fb2f0b396713"),
                            RoleId = new Guid("f169c95b-c9e4-4ab8-b03d-8884968dd711")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

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
