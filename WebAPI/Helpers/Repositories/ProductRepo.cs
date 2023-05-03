using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories
{
    public class ProductRepo : CosmosRepo<ProductEntity>
	{

		public ProductRepo(CosmosContext context) : base(context)
		{

		}
	}
}
