using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class ProductSchema
	{
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public string ImageUrl { get; set; } = null!;
		public TagEntity Tag { get; set; } = null!;
		public CategoryEntity Category { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int Rating { get; set; }
		public string Brand { get; set; } = null!;

		public static implicit operator ProductEntity(ProductSchema schema)
		{
			return new ProductEntity
			{
				Name = schema.Name,
				Price = schema.Price,
				ImageUrl = schema.ImageUrl,
				Description = schema.Description,
				Rating = schema.Rating,
				Brand = schema.Brand,
				Category = schema.Category,
				Tag = schema.Tag
			};
		}
	}
}
