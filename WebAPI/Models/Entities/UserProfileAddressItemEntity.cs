using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

[PrimaryKey(nameof(UserProfileId), nameof(AddressItemId))]
public class UserProfileAddressItemEntity
{
    [Required]
    public string UserProfileId { get; set; } = null!;

    public UserProfileEntity UserProfile { get; set; } = null!;

    [Required]
    public int AddressItemId { get; set; }

    public AddressItemEntity AddressItem { get; set; } = null!;

}

