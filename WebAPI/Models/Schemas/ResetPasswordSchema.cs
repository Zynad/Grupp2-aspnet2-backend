using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class ResetPasswordSchema
{
    [Required]
    public string Email { get; set; } = null!;
}
