using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace News_Site.Data;

public partial class NewsSiteContext : DbContext
{
    public NewsSiteContext()
    {
    }

    public NewsSiteContext(DbContextOptions<NewsSiteContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AllNews> AllNews { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<TargetKeyword> TargetKeywords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllNews>(entity =>
        {
            entity.ToTable("All News");

            entity.Property(e => e.AuthorName)
                .HasMaxLength(50)
                .HasColumnName("Author Name");
            entity.Property(e => e.AuthorPhoto)
                .HasMaxLength(50)
                .HasColumnName("Author Photo");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date Created");
            entity.Property(e => e.MainPhoto)
                .HasMaxLength(50)
                .HasColumnName("Main Photo");
            entity.Property(e => e.SId)
                .HasMaxLength(50)
                .HasColumnName("sId");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(50);
            entity.Property(e => e.Video).HasMaxLength(50);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Articles__3214EC07214AA868");

            entity.Property(e => e.Category)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.Keywords)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MainPhoto)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Main_Photo");
            entity.Property(e => e.PostedAt)
                .HasColumnType("datetime")
                .HasColumnName("Posted_at");
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.Url).IsUnicode(false);
            entity.Property(e => e.Website)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TargetKeyword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TargetKe__3214EC077EED8AA7");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
