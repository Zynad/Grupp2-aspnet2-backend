using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class TagEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(30)] 
    public string Name { get; set; } = null!;

    public string PartitionKey { get; set; } = "Tag";
}