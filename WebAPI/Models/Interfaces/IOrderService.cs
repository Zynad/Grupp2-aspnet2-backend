using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(OrderSchema schema, string userEmail);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<OrderDTO>> GetByUserIdAsync(Guid userId);
		Task<IEnumerable<OrderDTO>> GetBySignedInUser(string userEmail);
		Task<bool> CancelOrder(OrderCancelSchema schema);
        Task<bool> DeleteOrder(Guid orderId);

	}
}