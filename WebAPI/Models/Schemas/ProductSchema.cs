using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class ProductSchema
	{
		[Required]
		public string Name { get; set; } = null!;
		[Required]
		public double Price { get; set; }
		[Required]
		public string ImageUrl { get; set; } = null!;
		public List<string>? Tags { get; set; }
		public string? Category { get; set; }
		[Required]
		public string Description { get; set; } = null!;
		[Required]
		public string Brand { get; set; } = null!;
		public string? SalesCategory { get; set; }


		public static implicit operator ProductEntity(ProductSchema schema)
		{
			
			var salesCat = schema.SalesCategory.IsNullOrEmpty() ? "New" : schema.SalesCategory;
			
			return new ProductEntity
			{ 
				Id = Guid.NewGuid(),
				Name = schema.Name,
				Price = schema.Price,
				ImageUrl = schema.ImageUrl,
				Description = schema.Description,
				Brand = schema.Brand,
				Category = schema.Category,
				SalesCategory = salesCat,
				Tags = schema.Tags,
			};
		}
	}
}
