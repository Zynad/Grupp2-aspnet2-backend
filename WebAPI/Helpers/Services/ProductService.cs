using WebAPI.Models.Entities;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Helpers.Services
{
	public class ProductService
	{
		private readonly ProductRepo _productRepo;
		private readonly ReviewRepo _reviewRepo;

		public ProductService(ProductRepo productRepo, ReviewRepo reviewRepo)
		{
			_productRepo = productRepo;
			_reviewRepo = reviewRepo;
		}

		public async Task<IEnumerable<ProductDTO>> GetAllAsync()
		{
			try
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
			catch { }
			return null!;
		}

		public async Task<IEnumerable<ProductDTO>> GetBySalesCategoryAsync(string salesCategory)
		{
			try
			{
				var allProducts = await _productRepo.GetAllAsync();
				var products = allProducts.Where(x => x.SalesCategory == salesCategory);
				var dto = new List<ProductDTO>();

				foreach (var entity in products)
				{
					dto.Add(entity);
				}

				return dto;
			}
			catch { }
			return null!;
		}

		public async Task<IEnumerable<ProductDTO>> GetByTagAsync(List<string> tags)
		{
			try
			{
				var allProducts = await _productRepo.GetAllAsync();
				//This retrieves all products that match any of the tags in the input instead of only products that match all tags as the query below does
				//var products = allProducts.Where(x => x.Tags.Intersect(tags).Any());  
				var products = allProducts.Where(p => tags.All(t => p.Tags!.Contains(t)));
				var dto = new List<ProductDTO>();
				foreach (var entity in products)
				{
					dto.Add(entity);
				}

				return dto;
			}
			catch { }
			return null!;
		}

		public async Task<IEnumerable<ProductDTO>> GetByCategoryAsync(string category)
		{
			try
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
			catch { }
			return null!;
		}

		public async Task<ProductDTO> GetByIdAsync(Guid id)
		{
			var product = await _productRepo.GetAsync(x => x.Id == id);
			ProductDTO dto = product;

			return dto;
		}

		public async Task<IEnumerable<ProductDTO>> GetByPriceAsync(int minPrice, int maxPrice)
		{
			try
			{
				var products = await _productRepo.GetListAsync(x => x.Price >= minPrice && x.Price <= maxPrice);
				var dto = new List<ProductDTO>();

				foreach (var entity in products)
				{
					dto.Add(entity);
				}

				return dto;
			}
			catch { }
			return null!;
		}

		public async Task<IEnumerable<ProductDTO>> GetByNameAsync(string searchCondition)
		{
			try
			{
				Expression<Func<ProductEntity, bool>> predicate = p => p.Name.ToLower().Contains(searchCondition.ToLower());
				var products = await _productRepo.GetListAsync(predicate);

				return products.Select(p => (ProductDTO)p);
			}
			catch { }
			return null!;
		}

		public async Task<bool> CreateAsync(ProductSchema schema)
		{
			try
			{
				ProductEntity entity = schema;
				await _productRepo.AddAsync(entity);

				return true;
			}
			catch { }
			return false;
		}

		public async Task<bool> UpdateAsync(ProductSchema schema)
		{
			try
			{
				ProductEntity entity = schema;
				await _productRepo.UpdateAsync(entity);

				return true;
			}
			catch { }
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			try
			{
				var entity = await _productRepo.GetAsync(x => x.Id == id);
				await _productRepo.DeleteAsync(entity);

				return true;
			}
			catch { }		
			return false;
		}

		public async Task<bool> UpdateRatingAsync(Guid productId)
		{
			try
			{
				var ratings = new List<double>();
				var product = await _productRepo.GetAsync(p => p.Id == productId);
				var reviews = await _reviewRepo.GetListAsync(r => r.ProductId == productId);

				foreach (var review in reviews)
				{
					ratings.Add(review.Rating);
				}

				double count = ratings.Count;
				if (count > 0)
				{
					product.Rating = ratings.Sum() / count;
					product.ReviewCount = ratings.Count;
					await _productRepo.UpdateAsync(product);

					return true;
				}
			}
			catch { }
			return false;
		}

		public async Task<IEnumerable<ProductDTO>> GetFilteredProductsAsync(string? name, int? minPrice, int? maxPrice, string? tagsString, string? category, string? salesCategory, int? amount)
		{

			try
			{
				var products = await _productRepo.GetAllAsync();
				var dtos = new List<ProductDTO>();
				var tags = new List<string>();

				if (!string.IsNullOrEmpty(tagsString))
				{
					tags = tagsString.Split(',').Select(t => t.Trim()).ToList();
				}

				foreach (var product in products)
				{
					dtos.Add(product);
				}

				if (minPrice != null)
				{
					dtos = dtos.Where(p => p.Price >= minPrice).ToList();
				}
				if (maxPrice != null)
				{
					dtos = dtos.Where(p => p.Price <= maxPrice).ToList();
				}
				if (tags.Any() && tags.Count > 0)
				{
					dtos = dtos.Where(p => tags.All(t => p.Tags != null && p.Tags.Any(pt => string.Equals(pt, t, StringComparison.OrdinalIgnoreCase)))).ToList();
					// dtos = dtos.Where(p => tags.All(t => p.Tags!.Contains(t))).ToList();
				}
				if (!string.IsNullOrEmpty(name))
				{
					dtos = dtos.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
				}
				if (!string.IsNullOrEmpty(category))
				{
					dtos = dtos.Where(p => p.Category == category).ToList();
				}
				if (!string.IsNullOrEmpty(salesCategory))
				{
					dtos = dtos.Where(p => p.SalesCategory == salesCategory).ToList();
				}

				if (amount.HasValue)
					dtos = dtos.Take(amount.Value).ToList();
									
				return dtos;
			}
			catch { }
			return null!;
		}
	}
}
