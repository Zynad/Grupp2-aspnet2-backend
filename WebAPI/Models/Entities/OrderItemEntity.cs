namespace WebAPI.Models.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public double UnitPrice { get; set; }
    public int Quantity { get; set; }
}