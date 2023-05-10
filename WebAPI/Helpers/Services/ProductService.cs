using WebAPI.Models.Entities;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;

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
				//ProductDTO product = entity;
				dtos.Add(entity);
			}

			return dtos;
		}

		public async Task<IEnumerable<ProductDTO>> GetBySalesCategoryAsync(string salesCategory)
		{
			var allProducts = await _productRepo.GetAllAsync();

			var products = allProducts.Where(x => x.SalesCategory == salesCategory);

			var dto = new List<ProductDTO>();

			foreach(var entity in products)
			{
				dto.Add(entity);
			}
			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByTagAsync(List<string> tags)
		{
			var allProducts = await _productRepo.GetAllAsync();

			//This retrieves all products that match any of the tags in the input instead of only products that match all tags as the query below does
			//var products = allProducts.Where(x => x.Tags.Intersect(tags).Any());  

			var products = allProducts.Where(p => tags.All(t => p.Tags.Contains(t)));

			var dto = new List<ProductDTO>();

			foreach (var entity in products)
			{
				dto.Add(entity);
			}

			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByCategoryAsync(string category)
		{
			var allProducts = await _productRepo.GetAllAsync();

			var products = allProducts.Where(x => x.Category == category);

			var dto = new List<ProductDTO>();

			foreach (var entity in products)
			{
				dto.Add(entity);
			}
			return dto;
		}

		public async Task<ProductDTO> GetByIdAsync(Guid id)
		{
			var product = await _productRepo.GetAsync(x => x.Id == id);

			ProductDTO dto = product;

			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByPriceAsync(int minPrice, int maxPrice)
		{
			var products = await _productRepo.GetListAsync(x => x.Price >= minPrice && x.Price <= maxPrice);

			var dto = new List<ProductDTO>();

			foreach(var entity in products)
			{
				dto.Add(entity);
			}

			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByNameAsync(string searchCondition)
		{
			Expression<Func<ProductEntity, bool>> predicate = p => p.Name.ToLower().Contains(searchCondition.ToLower());
			var products = await _productRepo.GetListAsync(predicate);
			return products.Select(p => (ProductDTO)p);
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

		public async Task<bool> DeleteAsync(Guid id)
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

		//Hämta alla produkter i metoden eller ta en lista på produkter som redan valts från annat?
		public async Task<IEnumerable<ProductDTO>> GetFilteredProductsAsync(int? minPrice, int? maxPrice, List<string>? tags, string? category, string? salesCategory)
		{
			var products = await _productRepo.GetAllAsync();
			var dtos = new List<ProductDTO>();
			
			foreach (var product in products)
			{
				dtos.Add(product);
			}

			if(minPrice != null)
			{
				dtos = dtos.Where(p => p.Price >= minPrice).ToList();
			}
			if(maxPrice != null)
			{
				dtos = dtos.Where(p => p.Price <= maxPrice).ToList();
			}
			if(tags != null && tags.Count > 0)
			{
				dtos = dtos.Where(p => tags.All(t => p.Tags.Contains(t))).ToList();
			}
			if(!string.IsNullOrEmpty(category))
			{
				dtos = dtos.Where(p => p.Category == category).ToList();
			}
			if(!string.IsNullOrEmpty(salesCategory))
			{
				dtos = dtos.Where(p => p.SalesCategory == salesCategory).ToList();
			}
			
			return dtos;
		}
	}
}
