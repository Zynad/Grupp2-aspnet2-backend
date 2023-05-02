using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class TagEntity
{
    [Key] 
    public int Id { get; set; }

    [MaxLength(30)] 
    public string Name { get; set; } = null!;
}