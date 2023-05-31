using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
	public class OrderDTO
	{
		public Guid Id { get; set; }
		public DateTime OrderDate { get; set; }
		public string? OrderStatus { get; set; }
		public AddressEntity? Address { get; set; }
		public decimal Price { get; set; }
		public List<OrderItemEntity> Items { get; set; } = null!;

		public static implicit operator OrderDTO(OrderEntity entity)
		{
			return new OrderDTO
			{
				Id = entity.Id,
				OrderDate = entity.OrderDate,
				OrderStatus = entity.OrderStatus,
				Address = entity.Address,
				Items = entity.Items,
				Price = entity.Price,
			};
		}

	}
}
