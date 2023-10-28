using Database.Migrations.Entities;
using Microsoft.EntityFrameworkCore;
using LoveLetter.Core.Constants;
using System.Data;

namespace Database.Migrations.Configuration
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameStateEntity> Games { get; set; }

        public DbSet<LobbyEntity> Lobbies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = 
                string.IsNullOrEmpty(ConfigurationUtils.GetRemoteConnectionString()) ? 
                ConfigurationUtils.GetLocalConnectionString() 
                : ConfigurationUtils.GetRemoteConnectionString();

            optionsBuilder
                .UseSqlServer(connectionString)
                .UseValidationCheckConstraints()
                .UseEnumCheckConstraints();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LobbyEntity>()
              .ToTable(Tables.Lobbies);

            modelBuilder.Entity<LobbyEntity>()
              .HasKey(e => e.Id);

            modelBuilder.Entity<GameStateEntity>()
              .ToTable(Tables.States);

            modelBuilder.Entity<GameStateEntity>()
              .HasKey(e => e.Id);

            modelBuilder.Entity<GameStateEntity>()
              .HasOne(e => e.Lobby)
              .WithOne(e => e.State)
              .HasForeignKey<GameStateEntity>(e => e.Id)
              .HasConstraintName("FK_LobbyState")
              .IsRequired();

            modelBuilder.Entity<AuditItemEntity>()
              .ToTable(Tables.Audit);

            modelBuilder.Entity<AuditItemEntity>()
              .HasKey(e => e.Id);

            modelBuilder.Entity<AuditItemEntity>()
              .HasOne(e => e.GameState)
              .WithMany(e => e.AuditItems)
              .HasForeignKey(e => e.GameStateId)
              .HasConstraintName("FK_StateAuditItems")
              .IsRequired();
        }
    }
}
