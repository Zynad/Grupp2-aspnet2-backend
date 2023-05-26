using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface IReviewService
    {
        Task<bool> CreateAsync(ReviewSchema schema, string userName);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ReviewDTO>> GetAllAsync();
        Task<IEnumerable<ReviewDTO>> GetByProductId(Guid productId);
    }
}