using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;

namespace WebAPI.Contexts
{
	public class CosmosContext : DbContext
	{
		public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductEntity>()
				.ToContainer("Products")
				.HasPartitionKey(x => x.PartitionKey);

			modelBuilder.Entity<CategoryEntity>()
				.ToContainer("Categories")
				.HasPartitionKey(x => x.PartitionKey);

			modelBuilder.Entity<TagEntity>()
				.ToContainer("Tags")
				.HasPartitionKey(x => x.PartitionKey);
            modelBuilder.Entity<CouponEntity>()
                .ToContainer("Coupons")
                .HasPartitionKey(x => x.PartitionKey);
            modelBuilder.Entity<UserCouponEntity>()
                .ToContainer("UserCoupons")
                .HasPartitionKey(x => x.PartitionKey);
            modelBuilder.Entity<ReviewEntity>()
				.ToContainer("Reviews")
				.HasPartitionKey(x => x.PartitionKey);
		}
	}
}
