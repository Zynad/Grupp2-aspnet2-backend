using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas;

public class RegisterCreditCardSchema
{
    [Required]
    [MinLength(4)]
    public string NameOnCard { get; set; } = null!;

    [Required]
    [StringLength(16)]
    public string CardNo { get; set; } = null!;

    [Required]
    public int ExpireMonth { get; set; }

    [Required]
    public int ExpireYear { get; set; }

    [Required]
    [StringLength(3)]
    public string CVV { get; set; } = null!;

    public static implicit operator CreditCardEntity(RegisterCreditCardSchema schema)
    {
        return new CreditCardEntity
        {
            NameOnCard = schema.NameOnCard,
            CardNo = schema.CardNo,
            Expires = new DateTime(schema.ExpireYear, schema.ExpireMonth, 1),
            CVV = schema.CVV,
        };
    }
}
