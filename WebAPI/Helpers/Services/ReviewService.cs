using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class ReviewService
{
    private readonly ReviewRepo _reviewRepo;
    private readonly ProductService _productService;

    public ReviewService(ReviewRepo reviewRepo, ProductService productService)
    {
        _reviewRepo = reviewRepo;
        _productService = productService;
    }

    public async Task<IEnumerable<ReviewDTO>> GetAllAsync()
    {
        try
        {
			var products = await _reviewRepo.GetAllAsync();
			var dtos = new List<ReviewDTO>();

			foreach (var entity in products)
			{
				dtos.Add(entity);
			}

			return dtos;
		}
        catch { }
        return null!;
    }

    public async Task<IEnumerable<ReviewDTO>> GetByProductId(Guid productId)
    {
        try
        {
			var reviews = await _reviewRepo.GetListAsync(p => p.ProductId == productId);

			return reviews.Select(p => (ReviewDTO)p);
		}
        catch { }
        return null!;
    }

    public async Task<bool> CreateAsync(ReviewSchema schema)
    {
        try
        {
			ReviewEntity entity = schema;
			await _reviewRepo.AddAsync(entity);
            await _productService.UpdateRatingAsync(entity.ProductId);

            return true;
        }
        catch { }
        return false;   
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
			var entity = await _reviewRepo.GetAsync(r => r.Id == id);
			await _reviewRepo.DeleteAsync(entity!);

            return true;
        }
        catch { }
        return false;
    }
}