using WebAPI.Helpers.Services;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Schemas
{
	public class OrderItemSchema
	{
		//private readonly IProductService _productService;

		public Guid ProductId { get; set; }
		public string Color { get; set; } = null!;
		public string Size { get; set; } = null!;
		public int Quantity { get; set; }

	}
}
