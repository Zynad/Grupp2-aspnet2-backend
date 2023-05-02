using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;

namespace WebAPI.Contexts;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>()
            .ToContainer("Products")
            .HasPartitionKey(x => x.PartitionKey);
        
        modelBuilder.Entity<CategoryEntity>()
            .ToContainer("Categories")
            .HasPartitionKey(x => x.Id);
        
        modelBuilder.Entity<TagEntity>()
            .ToContainer("Tags")
            .HasPartitionKey(x => x.Id);
    }
}
