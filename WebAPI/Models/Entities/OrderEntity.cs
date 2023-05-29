namespace WebAPI.Models.Entities;

public class OrderEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? OrderStatus { get; set; }
    public AddressEntity Address { get; set; } = null!;
    public List<OrderItemEntity> Items { get; set; } = null!;

    public string PartitionKey = "Order";
}