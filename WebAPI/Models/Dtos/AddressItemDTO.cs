using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos;

public class AddressItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public AddressEntity Address { get; set; } = null!;
}
