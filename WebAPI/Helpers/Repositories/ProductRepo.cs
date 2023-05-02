using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Contexts;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories
{
	public class ProductRepo : Repo<ProductEntity>
	{

		private readonly DataContext _context;

		public ProductRepo(DataContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
		{
			return await _context.Products.Include("Category").Include("Tag").ToListAsync();
		}

		public override async Task<IEnumerable<ProductEntity>> GetListAsync(Expression<Func<ProductEntity, bool>> predicate)
		{
			return await _context.Products.Include("Category").Include("Tag").Where(predicate).ToListAsync();
		}

		public override async Task<ProductEntity> GetAsync(Expression<Func<ProductEntity, bool>> predicate)
		{
			var result = await _context.Products.Include("Category").Include("Tag").FirstOrDefaultAsync(predicate);

			if (result != null)
				return result;

			return null!;
		}
	}
}
