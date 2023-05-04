using WebAPI.Models.Entities;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;
using System.Linq.Expressions;

namespace WebAPI.Helpers.Services
{
	public class ProductService
	{
		private readonly ProductRepo _productRepo;
		private readonly CategoryRepo _categoryRepo;
		private readonly TagRepo _tagRepo;

		public ProductService(ProductRepo productRepo, CategoryRepo categoryRepo, TagRepo tagRepo)
		{
			_productRepo = productRepo;
			_categoryRepo = categoryRepo;
			_tagRepo = tagRepo;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllAsync()
		{
			var products = await _productRepo.GetAllAsync();

			var dtos = new List<ProductDTO>();

			foreach (var entity in products)
			{
				ProductDTO product = entity;
				dtos.Add(product);
			}

			return dtos;
		}

		public async Task<IEnumerable<ProductDTO>> GetByTagAsync(string tag)
		{
			var products = await _productRepo.GetListAsync(x => x.Tags.Contains(tag));

			var dto = new List<ProductDTO>();

			foreach (var entity in products)
			{
				ProductDTO product = entity;
				dto.Add(product);
			}

			return dto;
		}

		public async Task<ProductDTO> GetByIdAsync(Guid id)
		{
			var product = await _productRepo.GetAsync(x => x.Id == id);

			ProductDTO dto = product;

			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByNameAsync(string searchCondition)
		{
			Expression<Func<ProductEntity, bool>> predicate = p => p.Name.Contains(searchCondition);
			var products = await _productRepo.GetListAsync(predicate);
			return (IEnumerable<ProductDTO>)products;
		}

		public async Task<bool> CreateAsync(ProductSchema schema)
		{
			ProductEntity entity = schema;

			try
			{
				await _productRepo.AddAsync(entity);
				return true;
			}
			catch
			{
				return false;
			}

		}

		public async Task<bool> DeleteAsync(int id)
		{
			var entity = await _productRepo.GetAsync(x => x.Id == id);

			try
			{
				await _productRepo.DeleteAsync(entity!);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
