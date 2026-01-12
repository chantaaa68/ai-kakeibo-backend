using AiKakeiboBackend.Models;
using AiKakeiboBackend.Models.Interface;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Data;

public class KakeiboDbContext : DbContext
{
    public KakeiboDbContext()
    {
    }

    public KakeiboDbContext(DbContextOptions<KakeiboDbContext> options) : base(options) { }

    public DbSet<Users> Users { get; set; }
    public DbSet<Kakeibo> Kakeibo { get; set; }
    public DbSet<KakeiboItemFrequency> KakeiboItemFrequency { get; set; }
    public DbSet<KakeiboItem> KakeiboItem { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryDefault> CategoryDefault { get; set; }
    public DbSet<Icon> Icon { get; set; }
    public DbSet<NewsletterTemplate> NewsletterTemplate { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // joinするときに、delete_date==nullを接続するようにする
        modelBuilder.Entity<Icon>().HasQueryFilter(i => i.DeleteDate == null);
        modelBuilder.Entity<Users>().HasIndex(e => e.Email).IsUnique();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<KakeiboInterface>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateDate = DateTime.UtcNow.AddHours(9);
                entry.Entity.UpdateDate = DateTime.UtcNow.AddHours(9);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateDate = DateTime.UtcNow.AddHours(9);
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
