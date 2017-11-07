using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QueryExpressionTest.Models
{
    public partial class TestDbContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Repository> Repository { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamMember> TeamMember { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=../db.sqlite3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("CHAR(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("account_name")
                    .HasColumnType("VARCHAR(16)");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.ToTable("repository");

                entity.Property(e => e.RepositoryId)
                    .HasColumnName("repository_id")
                    .HasColumnType("CHAR(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasColumnName("owner_id")
                    .HasColumnType("CHAR(36)");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasColumnType("VARCHAR(16)");

                entity.Property(e => e.Visibility)
                    .IsRequired()
                    .HasColumnName("visibility")
                    .HasColumnType("VARCHAR(10)");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Repository)
                    .HasForeignKey(d => d.OwnerId);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("team");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("CHAR(36)")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Team)
                    .HasForeignKey<Team>(d => d.AccountId);
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.MemberId });

                entity.ToTable("team_member");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasColumnType("CHAR(36)");

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id")
                    .HasColumnType("CHAR(36)");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.MemberId);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.TeamId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("user");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("CHAR(36)")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.AccountId);
            });
        }
    }
}
