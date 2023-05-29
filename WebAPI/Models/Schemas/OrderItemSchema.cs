using WebAPI.Helpers.Services;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class OrderItemSchema
	{
		private readonly ProductService _productService;

		public Guid ProductId { get; set; }
		public string Color { get; set; } = null!;
		public string Size { get; set; } = null!;
		public int Quantity { get; set; }

		public OrderItemSchema(ProductService productService)
		{
			_productService = productService;
		}

		public async Task<OrderItemEntity> ToEntityAsync()
		{
			var product = await _productService.GetByIdAsync(ProductId);

			return new OrderItemEntity
			{
				ProductId = ProductId,
				ProductName = product.Name,
				UnitPrice = product.Price,
				ImageUrl = product.ImageUrl,
				Color = Color,
				Size = Size,
				Quantity = Quantity,
			};
		}

		//public static implicit operator OrderItemEntity(OrderItemSchema schema)
		//{
		//	return schema.ToEntityAsync().GetAwaiter().GetResult();
		//}
	}
}
