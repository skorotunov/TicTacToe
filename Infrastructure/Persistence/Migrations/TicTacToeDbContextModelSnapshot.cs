﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicTacToe.Infrastructure.Persistence;

namespace TicTacToe.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(TicTacToeDbContext))]
    partial class TicTacToeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.CrossPlayerGameTile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<byte>("TileId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("TileId");

                    b.ToTable("CrossPlayerGameTiles");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CrossPlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("NoughtPlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("TurnNumber")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.NoughtPlayerGameTile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<byte>("TileId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("TileId");

                    b.ToTable("NoughtPlayerGameTiles");
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.Tile", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<byte>("X")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Y")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Tiles");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            X = (byte)0,
                            Y = (byte)0
                        },
                        new
                        {
                            Id = (byte)2,
                            X = (byte)0,
                            Y = (byte)1
                        },
                        new
                        {
                            Id = (byte)3,
                            X = (byte)0,
                            Y = (byte)2
                        },
                        new
                        {
                            Id = (byte)4,
                            X = (byte)1,
                            Y = (byte)0
                        },
                        new
                        {
                            Id = (byte)5,
                            X = (byte)1,
                            Y = (byte)1
                        },
                        new
                        {
                            Id = (byte)6,
                            X = (byte)1,
                            Y = (byte)2
                        },
                        new
                        {
                            Id = (byte)7,
                            X = (byte)2,
                            Y = (byte)0
                        },
                        new
                        {
                            Id = (byte)8,
                            X = (byte)2,
                            Y = (byte)1
                        },
                        new
                        {
                            Id = (byte)9,
                            X = (byte)2,
                            Y = (byte)2
                        });
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.WinCondition", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("WinConditions");

                    b.HasData(
                        new
                        {
                            Id = (byte)1
                        },
                        new
                        {
                            Id = (byte)2
                        },
                        new
                        {
                            Id = (byte)3
                        },
                        new
                        {
                            Id = (byte)4
                        },
                        new
                        {
                            Id = (byte)5
                        },
                        new
                        {
                            Id = (byte)6
                        },
                        new
                        {
                            Id = (byte)7
                        },
                        new
                        {
                            Id = (byte)8
                        });
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.WinConditionTile", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<byte>("TileId")
                        .HasColumnType("tinyint");

                    b.Property<byte>("WinConditionId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("TileId");

                    b.HasIndex("WinConditionId");

                    b.ToTable("WinConditionTiles");

                    b.HasData(
                        new
                        {
                            Id = (byte)1,
                            TileId = (byte)1,
                            WinConditionId = (byte)1
                        },
                        new
                        {
                            Id = (byte)2,
                            TileId = (byte)4,
                            WinConditionId = (byte)1
                        },
                        new
                        {
                            Id = (byte)3,
                            TileId = (byte)7,
                            WinConditionId = (byte)1
                        },
                        new
                        {
                            Id = (byte)4,
                            TileId = (byte)2,
                            WinConditionId = (byte)2
                        },
                        new
                        {
                            Id = (byte)5,
                            TileId = (byte)5,
                            WinConditionId = (byte)2
                        },
                        new
                        {
                            Id = (byte)6,
                            TileId = (byte)8,
                            WinConditionId = (byte)2
                        },
                        new
                        {
                            Id = (byte)7,
                            TileId = (byte)3,
                            WinConditionId = (byte)3
                        },
                        new
                        {
                            Id = (byte)8,
                            TileId = (byte)6,
                            WinConditionId = (byte)3
                        },
                        new
                        {
                            Id = (byte)9,
                            TileId = (byte)9,
                            WinConditionId = (byte)3
                        },
                        new
                        {
                            Id = (byte)10,
                            TileId = (byte)1,
                            WinConditionId = (byte)4
                        },
                        new
                        {
                            Id = (byte)11,
                            TileId = (byte)2,
                            WinConditionId = (byte)4
                        },
                        new
                        {
                            Id = (byte)12,
                            TileId = (byte)3,
                            WinConditionId = (byte)4
                        },
                        new
                        {
                            Id = (byte)13,
                            TileId = (byte)4,
                            WinConditionId = (byte)5
                        },
                        new
                        {
                            Id = (byte)14,
                            TileId = (byte)5,
                            WinConditionId = (byte)5
                        },
                        new
                        {
                            Id = (byte)15,
                            TileId = (byte)6,
                            WinConditionId = (byte)5
                        },
                        new
                        {
                            Id = (byte)16,
                            TileId = (byte)7,
                            WinConditionId = (byte)6
                        },
                        new
                        {
                            Id = (byte)17,
                            TileId = (byte)8,
                            WinConditionId = (byte)6
                        },
                        new
                        {
                            Id = (byte)18,
                            TileId = (byte)9,
                            WinConditionId = (byte)6
                        },
                        new
                        {
                            Id = (byte)19,
                            TileId = (byte)1,
                            WinConditionId = (byte)7
                        },
                        new
                        {
                            Id = (byte)20,
                            TileId = (byte)5,
                            WinConditionId = (byte)7
                        },
                        new
                        {
                            Id = (byte)21,
                            TileId = (byte)9,
                            WinConditionId = (byte)7
                        },
                        new
                        {
                            Id = (byte)22,
                            TileId = (byte)3,
                            WinConditionId = (byte)8
                        },
                        new
                        {
                            Id = (byte)23,
                            TileId = (byte)5,
                            WinConditionId = (byte)8
                        },
                        new
                        {
                            Id = (byte)24,
                            TileId = (byte)7,
                            WinConditionId = (byte)8
                        });
                });

            modelBuilder.Entity("TicTacToe.Infrastructure.Identity.TicTacToeUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TicTacToe.Infrastructure.Identity.TicTacToeUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TicTacToe.Infrastructure.Identity.TicTacToeUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Infrastructure.Identity.TicTacToeUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TicTacToe.Infrastructure.Identity.TicTacToeUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.CrossPlayerGameTile", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.Game", "Game")
                        .WithMany("CrossPlayerGameTiles")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.Tile", "Tile")
                        .WithMany("CrossPlayerGameTiles")
                        .HasForeignKey("TileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.NoughtPlayerGameTile", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.Game", "Game")
                        .WithMany("NoughtPlayerGameTiles")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.Tile", "Tile")
                        .WithMany("NoughtPlayerGameTiles")
                        .HasForeignKey("TileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TicTacToe.Domain.Entities.WinConditionTile", b =>
                {
                    b.HasOne("TicTacToe.Domain.Entities.Tile", "Tile")
                        .WithMany("WinConditionTiles")
                        .HasForeignKey("TileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TicTacToe.Domain.Entities.WinCondition", "WinCondition")
                        .WithMany("WinConditionTiles")
                        .HasForeignKey("WinConditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
