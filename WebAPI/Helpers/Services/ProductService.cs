﻿using WebAPI.Models.Entities;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models.Interfaces;

namespace WebAPI.Helpers.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepo _productRepo;
        private readonly ReviewRepo _reviewRepo;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public ProductService(ProductRepo productRepo, ReviewRepo reviewRepo, ITagService tagService, ICategoryService categoryService)
        {
            _productRepo = productRepo;
            _reviewRepo = reviewRepo;
            _tagService = tagService;
            _categoryService = categoryService;
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
            try
            {
				var product = await _productRepo.GetAsync(x => x.Id == id);
				ProductDTO dto = product;

				return dto;
			}
            catch { }
            return null!;
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
                var tagList = await _tagService.CheckOrCreateAsync(schema.Tags!);
                if (tagList)
                {
                    var categoryResult = await _categoryService.CheckOrCreateAsync(schema.Category!);
                    if (categoryResult)
                    {
                        ProductEntity entity = schema;
                        await _productRepo.AddAsync(entity);

                        return true;
                    }
                } 
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

        public async Task<IEnumerable<ProductDTO>> GetFilteredProductsAsync(FilterSchema filter)
        {

            try
            {
                var products = await _productRepo.GetAllAsync();
                var dtos = new List<ProductDTO>();
                var tags = new List<string>();

                if (!string.IsNullOrEmpty(filter.Tags))
                {
                    tags = filter.Tags.Split(',').Select(t => t.Trim()).ToList();
                }

                foreach (var product in products)
                {
                    dtos.Add(product);
                }

                if (filter.MinPrice != null)
                {
                    dtos = dtos.Where(p => p.Price >= filter.MinPrice).ToList();
                }
                if (filter.MaxPrice != null)
                {
                    dtos = dtos.Where(p => p.Price <= filter.MaxPrice).ToList();
                }
                if (tags.Any() && tags.Count > 0)
                {
                    dtos = dtos.Where(p => tags.All(t => p.Tags != null && p.Tags.Any(pt => string.Equals(pt, t, StringComparison.OrdinalIgnoreCase)))).ToList();
                    // dtos = dtos.Where(p => tags.All(t => p.Tags!.Contains(t))).ToList();
                }
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    dtos = dtos.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(filter.Category))
                {
                    dtos = dtos.Where(p => p.Category == filter.Category).ToList();
                }
                if (!string.IsNullOrEmpty(filter.SalesCategory))
                {
                    dtos = dtos.Where(p => p.SalesCategory == filter.SalesCategory).ToList();
                }

                if (filter.Amount.HasValue)
                    dtos = dtos.Take(filter.Amount.Value).ToList();

                return dtos;
            }
            catch { }
            return null!;
        }
    }
}
