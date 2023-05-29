using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.CodeAnalysis;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Twilio.TwiML.Voice;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Email;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class OrderService : IOrderService
{
	private readonly OrderRepo _orderRepo;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IMailService _mailService;
	private readonly AddressRepo _addressRepo;
	private readonly IProductService _productService;

	public OrderService(OrderRepo orderRepo, UserManager<IdentityUser> userManager, IMailService mailService, AddressRepo addressRepo, IProductService productService)
	{
		_orderRepo = orderRepo;
		_userManager = userManager;
		_mailService = mailService;
		_addressRepo = addressRepo;
		_productService = productService;
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

	public async Task<OrderDTO> GetByOrderIdAsync(Guid orderId)
	{
		try
		{
			var order = await _orderRepo.GetAsync(x => x.Id == orderId);
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
			var currentDate = DateTime.Now;
			var dtos = new List<OrderDTO>();

			foreach (var item in orders)
			{
				var status = item.OrderStatus;
				var orderDate = item.OrderDate;

				TimeSpan diff = currentDate - orderDate;
				var daysDiff = diff.Days;

				if (status != "Cancelled" && status != "Delivered")
				{
					if (daysDiff > 1 && daysDiff < 3)
					{
						item.OrderStatus = "Shipped";
					}
					else if (daysDiff >= 3)
					{
						item.OrderStatus = "Delivered";
					}

					await _orderRepo.UpdateAsync(item);
				}

				dtos.Add(item);
			}

			return dtos;
		}
		catch { }
		return null!;
	}

	public async Task<bool> CancelOrder(Guid orderId)
	{
		try
		{
			var order = await _orderRepo.GetAsync(x => x.Id == orderId);
			order.OrderStatus = "Cancelled";

			await _orderRepo.UpdateAsync(order);

			return true;
		}
		catch { }
		return false;
	}

	public async Task<bool> CreateOrderAsync(OrderSchema schema, string userEmail)
	{
		try
		{
			var orderItems = schema.Items;
			var user = await _userManager.FindByEmailAsync(userEmail);
			var address = await _addressRepo.GetAsync(x => x.Id == schema.AddressId);

			var order = new OrderEntity
			{
				OrderDate = DateTime.Now,
				OrderStatus = "Pending",
				Items = new List<OrderItemEntity>(),
				UserId = Guid.Parse(user!.Id),
				Address = address
			};

			foreach (var item in orderItems)
			{
				var product = await _productService.GetByIdAsync(item.ProductId);

				var orderItem = new OrderItemEntity
				{
					ProductId = item.ProductId,
					ProductName = product.Name,
					UnitPrice = product.Price,
					ImageUrl = product.ImageUrl,
					Color = item.Color,
					Size = item.Size,
					Quantity = item.Quantity,
				};
				order.Items.Add(orderItem);
			}

			await _orderRepo.AddAsync(order);

			//var email = new MailData(new List<string> { userEmail }, "Order confirmation", $"Your order with Id: {order.Id} has been recieved! We will ship your items to you shortly.");
			//var result = await _mailService.SendAsync(email, new CancellationToken());

			return true;
		}
		catch { }
		return false;
	}

	public async Task<bool> DeleteOrder(Guid orderId)
	{
		try
		{
			var entity = await _orderRepo.GetAsync(x => x.Id == orderId);
			await _orderRepo.DeleteAsync(entity);

			return true;
		}
		catch { }
		return false;
	}
}