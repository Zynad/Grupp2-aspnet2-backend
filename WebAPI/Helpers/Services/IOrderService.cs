using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services
{
	public interface IOrderService
	{
		Task<bool> CreateOrderAsync(OrderSchema schema, string userEmail);
		Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
		Task<OrderDTO> GetByOrderIdAsync(Guid id);
		Task<IEnumerable<OrderDTO>> GetByUserIdAsync(Guid userId);
	}
}