using CatCafe.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CatCafe.Data;

public class CatCafeDbContext : IdentityDbContext
{
    public DbSet<Cat> Cats { get; set; } = null;

    public CatCafeDbContext(DbContextOptions<CatCafeDbContext> options)
        : base(options)
    {
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IEntity entity && entry.State == EntityState.Modified)
            {
                Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                entity.LastUpdated = DateTime.Now;
            }
        }

        return (await base.SaveChangesAsync(true, cancellationToken));
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IEntity entity && entry.State == EntityState.Modified)
            {
               entity.LastUpdated = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .Property(c => c.CreatedOn)
            .HasDefaultValueSql("getdate()");
        base.OnModelCreating(modelBuilder);
    }

}
