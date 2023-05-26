using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class OrderService
{
    //update order/update status
    //delete/cancel order

    private readonly ProductRepo _productRepo;
    private readonly OrderRepo _orderRepo;
    private readonly UserManager<IdentityUser> _userManager;

	public OrderService(ProductRepo productRepo, OrderRepo orderRepo, UserManager<IdentityUser> userManager)
	{
		_productRepo = productRepo;
		_orderRepo = orderRepo;
		_userManager = userManager;
	}

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
    {
        try
        {
            var orders = await _orderRepo.GetAllAsync();
            var dtos = new List<OrderDTO>();

            foreach (var entity in orders)
            {
				dtos.Add(entity);
            }

            return dtos;
        }
        catch { }
        return null!;
    }

    public async Task<OrderDTO> GetByOrderIdAsync(Guid id)
    {
        try
        {
            var order = await _orderRepo.GetAsync(x => x.Id == id);
            OrderDTO dto = order;

            return dto;
        }
        catch { }
        return null!;
    }

    public async Task<IEnumerable<OrderDTO>> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var orders = await _orderRepo.GetListAsync(x => x.UserId == userId);
            var dtos = new List<OrderDTO>();

            foreach (var entity in orders)
                dtos.Add(entity);

            return dtos;
        }
        catch { }
        return null!;
    }

	public async Task<bool> CreateOrderAsync(OrderSchema schema, string userEmail)
    {
		try
        {
			var orderItems = schema.Items;
			var user = await _userManager.FindByEmailAsync(userEmail);

			var order = new OrderEntity
            {
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                Items = new List<OrderItemEntity>(),
                UserId = Guid.Parse(user!.Id)
            };

            foreach (var item in orderItems)
            {
                OrderItemEntity OrderItem = item;
                order.Items.Add(OrderItem);
            }

            await _orderRepo.AddAsync(order);

            return true;
        }
        catch { }
        return false;
    }
    
}