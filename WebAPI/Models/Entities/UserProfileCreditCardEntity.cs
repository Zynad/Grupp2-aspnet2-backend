using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

[PrimaryKey(nameof(UserProfileId), nameof(CreditCardId))]
public class UserProfileCreditCardEntity
{
    [Required]
    public string UserProfileId { get; set; } = null!;

    public UserProfileEntity UserProfile { get; set; } = null!;

    [Required]
    public int CreditCardId { get; set; }

    public CreditCardEntity CreditCard { get; set; } = null!;
}
