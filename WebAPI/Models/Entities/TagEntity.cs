using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class TagEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(30)] 
    public string Name { get; set; } = null!;

    public string PartitionKey => Id.ToString()!;
}