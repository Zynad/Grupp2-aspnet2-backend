using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class OrderItemSchema
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public double UnitPrice { get; set; }
		public int Quantity { get; set; }

		public static implicit operator OrderItemEntity(OrderItemSchema schema)
		{
			return new OrderItemEntity
			{
				ProductId = schema.ProductId,
				ProductName = schema.ProductName,
				UnitPrice = schema.UnitPrice,
				Quantity = schema.Quantity,
			};
		}
	}
}
