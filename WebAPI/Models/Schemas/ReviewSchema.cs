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
        DateTime now = DateTime.Now;
        return new ReviewEntity
        {
            Id = Guid.NewGuid(),
            ProductId = schema.ProductId,
            Comment = schema.Comment,
            Rating = schema.Rating,
            CreatedDate = new DateTime(now.Year,now.Month,now.Day),
        };
    }
}