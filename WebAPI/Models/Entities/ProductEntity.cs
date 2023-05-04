using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Entities
{
	public class ProductEntity
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Category { get; set; }
		public List<string> Tags { get; set; }
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
		
		public string PartitionKey => Id.ToString();
	}
}
