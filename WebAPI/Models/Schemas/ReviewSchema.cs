using Microsoft.Build.Framework;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas;

public class ReviewSchema
{
    public Guid ProductId { get; set; }

    [Required]
    public string Comment { get; set; } = null!;
    [Required]
    public double Rating { get; set; }

    public static implicit operator ReviewEntity(ReviewSchema schema)
    {
        return new ReviewEntity
        {
            Id = Guid.NewGuid(),
            ProductId = schema.ProductId,
            Comment = schema.Comment,
            Rating = schema.Rating
        };
    }
}