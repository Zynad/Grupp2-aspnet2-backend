using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class OrderRepo : CosmosRepo<OrderEntity>
{
	public OrderRepo(CosmosContext context) : base(context)
	{
	}
}