using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos;

public class ReviewDTO
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }

    public string UserName { get; set; } = null!;

    public string Comment { get; set; } = null!;
    
    public double Rating { get; set; }

    public static implicit operator ReviewDTO(ReviewEntity entity)
    {
        return new ReviewDTO
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            UserName = entity.UserName,
            Comment = entity.Comment,
            Rating = entity.Rating
        };
    }
}