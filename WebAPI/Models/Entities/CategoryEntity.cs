namespace WebAPI.Models.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }  = Guid.NewGuid();
    
    public string Name { get; set; } = null!;

    public string PartitionKey => Id.ToString();
}