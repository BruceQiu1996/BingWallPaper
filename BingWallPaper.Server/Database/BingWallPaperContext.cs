using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BingWallPaper.Server.Database;

public partial class BingWallPaperContext : DbContext
{
    public BingWallPaperContext()
    {
    }

    public BingWallPaperContext(DbContextOptions<BingWallPaperContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Wallpaper> Wallpapers { get; set; }

    public virtual DbSet<WallpaperCn> WallpaperCns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Wallpaper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wallpaper");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Desc).HasMaxLength(500);
            entity.Property(e => e._4k)
                .HasMaxLength(255)
                .HasColumnName("4K");
        });

        modelBuilder.Entity<WallpaperCn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wallpaper_cn");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Desc).HasMaxLength(500);
            entity.Property(e => e._4k)
                .HasMaxLength(255)
                .HasColumnName("4K");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
