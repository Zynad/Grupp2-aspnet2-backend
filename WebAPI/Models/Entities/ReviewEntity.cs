using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class ReviewEntity
{
    [Key] 
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ProductId { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Comment { get; set; } = null!;

    [Required]
    public double Rating { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    public string PartitionKey { get; set; } = "Review";
}