using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BingWallPaper.Server.Database;

public partial class BingwallpaperContext : DbContext
{
    public BingwallpaperContext()
    {
    }

    public BingwallpaperContext(DbContextOptions<BingwallpaperContext> options)
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
            entity.Property(e => e.Desc).HasMaxLength(5000);
            entity.Property(e => e.UrlBase).HasMaxLength(255);
        });

        modelBuilder.Entity<WallpaperCn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("wallpaper_cn");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Desc).HasMaxLength(5000);
            entity.Property(e => e.UrlBase).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
