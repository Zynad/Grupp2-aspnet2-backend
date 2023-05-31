using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class OrderSchema
	{
		public int AddressId { get; set; }
		public List<OrderItemSchema> Items { get; set; } = null!;
		public decimal Price { get; set; }
	}
}
