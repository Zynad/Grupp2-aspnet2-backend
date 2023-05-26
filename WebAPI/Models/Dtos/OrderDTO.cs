using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
	public class OrderDTO
	{
		public Guid Id { get; set; }
		public DateTime OrderDate { get; set; }
		public string? OrderStatus { get; set; }
		public string? Address { get; set; }
		public List<OrderItemEntity> Items { get; set; } = null!;

	}
}
