using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Dtos;

namespace WebAPI.Models.Entities;

public class AddressItemEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(AddressId))]
    public int AddressId { get; set; }

    [Required]
    public AddressEntity Address { get; set; } = null!;

    public ICollection<UserProfileAddressItemEntity> UserProfileAddressItems { get; set; } = new HashSet<UserProfileAddressItemEntity>();


    public static implicit operator AddressItemDTO(AddressItemEntity entity)
    {
        return new AddressItemDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Address = entity.Address,
        };
    }
}
