using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
	public class ProductDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public string ImageUrl { get; set; } = null!;
		public List<string> Tags { get; set; } = null!;
		public string Category { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int Rating { get; set; }
		public string Brand { get; set; } = null!;
		public double? DiscountMultiplier { get; set; }


		public static implicit operator ProductDTO(ProductEntity entity)
		{
			return new ProductDTO
			{
				Id = entity.Id,
				Name = entity.Name,
				Price = entity.Price,
				ImageUrl = entity.ImageUrl,
				Tags = entity.Tags,
				Category = entity.Category,
				Description = entity.Description,
				Rating = entity.Rating,
				Brand = entity.Brand,

			};
		}
	}
}
