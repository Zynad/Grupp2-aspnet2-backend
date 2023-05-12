using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Entities
{
	public class ProductEntity
	{
		[Key]
		[Required]
		public Guid Id { get; set; } = Guid.NewGuid();

		public string? Category { get; set; }
		public List<string>? Tags { get; set; }

		public string SalesCategory { get; set; } = "New";
		
		[MaxLength(100)]
		[Required]
		public string Name { get; set; } = null!;

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		[Required]
		public double Price { get; set; }

		[Required]
		public string ImageUrl { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;

		public double Rating { get; set; } = 0;

		[Required]
		public string Brand { get; set; } = null!;

		public bool Discount { get; set; }

		public double? DiscountMultiplier { get; set; }

		public string PartitionKey { get; set; } = "Product";
	}
}
