using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class ChangePasswordSchema
{
    [Required]
    public string CurrentPassword { get; set; } = null!;
    [Required]
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$")]
    public string NewPassword { get; set; } = null!;
}
