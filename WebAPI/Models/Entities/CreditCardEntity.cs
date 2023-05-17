using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Dtos;

namespace WebAPI.Models.Entities;

public class CreditCardEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    public string NameOnCard { get; set; } = null!;

    [Required]
    [StringLength(16)]
    public string CardNo { get; set; } = null!;

    [Required]
    public DateTime Expires { get; set; }

    [Required]
    [StringLength(3)]
    public string CVV { get; set; } = null!;

    public ICollection<UserProfileCreditCardEntity> UserProfileCreditCards { get; set; } = new HashSet<UserProfileCreditCardEntity>();

    public static implicit operator CreditCardDTO(CreditCardEntity entity)
    {
        return new CreditCardDTO
        {
            NameOnCard = entity.NameOnCard,
            CardNo = entity.CardNo,
            ExpireYear = entity.Expires.Year,
            ExpireMonth = entity.Expires.Month,
            CVV = entity.CVV,
        };
    }
}
