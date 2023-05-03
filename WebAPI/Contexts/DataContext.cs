using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;

namespace WebAPI.Contexts;

public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    DbSet<UserProfileEntity> UserProfileEntities { get; set; }
}
