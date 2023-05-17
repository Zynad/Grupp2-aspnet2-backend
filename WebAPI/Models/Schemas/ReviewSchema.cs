using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas;

public class ReviewSchema
{
    public Guid ProductId { get; set; }
    public string UserName { get; set; } = null!;

    public string Comment { get; set; } = null!;
    
    public double Rating { get; set; }

    public static implicit operator ReviewEntity(ReviewSchema schema)
    {
        return new ReviewEntity
        {
            Id = Guid.NewGuid(),
            ProductId = schema.ProductId,
            UserName = schema.UserName,
            Comment = schema.Comment,
            Rating = schema.Rating
        };
    }
}