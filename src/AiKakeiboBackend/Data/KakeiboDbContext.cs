using AiKakeiboBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Data;

public class KakeiboDbContext : DbContext
{
    public KakeiboDbContext(DbContextOptions<KakeiboDbContext> options)
        : base(options)
    {
    }

    public DbSet<Users> Users { get; set; } = null!;
    public DbSet<Kakeibo> Kakeibos { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CategoryDefault> CategoryDefaults { get; set; } = null!;
    public DbSet<Icon> Icons { get; set; } = null!;
    public DbSet<KakeiboItem> KakeiboItems { get; set; } = null!;
    public DbSet<KakeiboItemFrequency> KakeiboItemFrequencies { get; set; } = null!;
    public DbSet<NewsletterTemplate> NewsletterTemplates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users configuration
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Users>()
            .HasIndex(u => u.UserHash)
            .IsUnique();

        // Kakeibo configuration
        modelBuilder.Entity<Kakeibo>()
            .HasOne(k => k.User)
            .WithMany(u => u.Kakeibos)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category configuration
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Kakeibo)
            .WithMany(k => k.Categories)
            .HasForeignKey(c => c.KakeiboID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Icon)
            .WithMany(i => i.Categories)
            .HasForeignKey(c => c.IconId)
            .OnDelete(DeleteBehavior.Restrict);

        // CategoryDefault configuration
        modelBuilder.Entity<CategoryDefault>()
            .HasOne(cd => cd.Icon)
            .WithMany(i => i.CategoryDefaults)
            .HasForeignKey(cd => cd.IconId)
            .OnDelete(DeleteBehavior.Restrict);

        // KakeiboItem configuration
        modelBuilder.Entity<KakeiboItem>()
            .HasOne(ki => ki.Kakeibo)
            .WithMany(k => k.KakeiboItems)
            .HasForeignKey(ki => ki.KakeiboId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KakeiboItem>()
            .HasOne(ki => ki.Category)
            .WithMany()
            .HasForeignKey(ki => ki.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<KakeiboItem>()
            .HasOne(ki => ki.KakeiboItemFrequency)
            .WithMany(kif => kif.KakeiboItems)
            .HasForeignKey(ki => ki.FrequencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // KakeiboItemFrequency configuration
        modelBuilder.Entity<KakeiboItemFrequency>()
            .HasOne(kif => kif.Kakeibo)
            .WithMany(k => k.KakeiboItemFrequencies)
            .HasForeignKey(kif => kif.KakeiboId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KakeiboItemFrequency>()
            .HasOne(kif => kif.Category)
            .WithMany()
            .HasForeignKey(kif => kif.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index for better query performance
        modelBuilder.Entity<KakeiboItem>()
            .HasIndex(ki => ki.UsedDate);

        modelBuilder.Entity<Kakeibo>()
            .HasIndex(k => k.UserId);
    }
}
