﻿// <auto-generated />
using System;
using Database.Migrations.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations.Migrations
{
    [DbContext(typeof(GameDbContext))]
    partial class GameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Database.Migrations.Entities.AuditItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameStateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerNickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("PlayerNumber")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("Audit", (string)null);
                });

            modelBuilder.Entity("Database.Migrations.Entities.GameStateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardHistory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Deck")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<string>("Players")
                        .HasColumnType("Xml");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("TurnPlayerNumber")
                        .HasColumnType("smallint");

                    b.Property<short?>("WinnerPlayerNumber")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("States", (string)null);
                });

            modelBuilder.Entity("Database.Migrations.Entities.LobbyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Players")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Lobbies", null, t =>
                        {
                            t.HasCheckConstraint("CK_Lobbies_Status_Enum", "[Status] IN (CAST(0 AS smallint), CAST(1 AS smallint), CAST(2 AS smallint))");
                        });
                });

            modelBuilder.Entity("Database.Migrations.Entities.AuditItemEntity", b =>
                {
                    b.HasOne("Database.Migrations.Entities.GameStateEntity", "GameState")
                        .WithMany("AuditItems")
                        .HasForeignKey("GameStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_StateAuditItems");

                    b.Navigation("GameState");
                });

            modelBuilder.Entity("Database.Migrations.Entities.GameStateEntity", b =>
                {
                    b.HasOne("Database.Migrations.Entities.LobbyEntity", "Lobby")
                        .WithOne("State")
                        .HasForeignKey("Database.Migrations.Entities.GameStateEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_LobbyState");

                    b.Navigation("Lobby");
                });

            modelBuilder.Entity("Database.Migrations.Entities.GameStateEntity", b =>
                {
                    b.Navigation("AuditItems");
                });

            modelBuilder.Entity("Database.Migrations.Entities.LobbyEntity", b =>
                {
                    b.Navigation("State");
                });
#pragma warning restore 612, 618
        }
    }
}
