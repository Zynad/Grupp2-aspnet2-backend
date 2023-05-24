using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Entities;

public class UserProfileEntity
{
    [Key, ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public ICollection<UserProfileAddressItemEntity> UserProfileAddressItems { get; set; } = new HashSet<UserProfileAddressItemEntity>();
    public ICollection<UserProfileCreditCardEntity> UserProfileCreditCards { get; set; } = new HashSet<UserProfileCreditCardEntity>();
}
