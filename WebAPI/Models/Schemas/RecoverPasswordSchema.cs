using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class RecoverPasswordSchema
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Token { get; set; } = null!;
    [Required]
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$")]
    public string Password { get; set; } = null!;
}
