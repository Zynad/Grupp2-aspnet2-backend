using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class CategoryEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; }  = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = null!;

    public string PartitionKey { get; set; } = "Category";
}