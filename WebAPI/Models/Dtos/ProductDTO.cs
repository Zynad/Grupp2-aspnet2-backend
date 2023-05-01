using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
	public class ProductDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public string ImageUrl { get; set; } = null!;
		//public string Tag { get; set; } = null!;
		//public string Category { get; set; } = null!;
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
				//Tag = entity.Tag.Name,
				//Category = entity.Category.Name,
				Description = entity.Description,
				Rating = entity.Rating,
				Brand = entity.Brand,

			};
		}
	}
}
