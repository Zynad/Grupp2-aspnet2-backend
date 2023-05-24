namespace WebAPI.Models.Schemas
{
	// string? name, int? minPrice, int? maxPrice, string? tags, string? category, string? salesCategory, int? amount
	public class FilterSchema
	{
		public string? Name { get; set; }
		public int? MinPrice { get; set; }
		public int? MaxPrice { get; set;}
		public string? Tags { get; set; }
		public string? Category { get; set; }
		public string? SalesCategory { get; set; }
		public int? Amount { get; set; }
	}
}
