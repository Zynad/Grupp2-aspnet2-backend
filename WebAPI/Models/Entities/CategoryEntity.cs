namespace WebAPI.Models.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    
    public string CategoryName { get; set; } = null!;

    public IEnumerable<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}