using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos;

public class ReviewDTO
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Comment { get; set; } = null!;
    
    public double Rating { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    public static implicit operator ReviewDTO(ReviewEntity entity)
    {
        return new ReviewDTO
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Name = entity.Name,
            Comment = entity.Comment,
            Rating = entity.Rating,
            CreatedDate = entity.CreatedDate,
            ImageUrl = entity.ImageUrl,
        };
    }
}