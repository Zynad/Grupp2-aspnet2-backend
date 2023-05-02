using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Entities
{
	public class ProductEntity
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("CategoryId")]
		public int CategoryId { get; set; }

		public CategoryEntity Category { get; set; }

		[ForeignKey("TagId")]
		public int TagId { get; set; }

		public TagEntity Tag { get; set; }

		[MaxLength(100)]
		public string Name { get; set; } = null!;

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public double Price { get; set; }

		public string ImageUrl { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int Rating { get; set; }

		public string Brand { get; set; } = null!;

		public bool Discount { get; set; }

		public double? DiscountMultiplier { get; set; }
	}
}
