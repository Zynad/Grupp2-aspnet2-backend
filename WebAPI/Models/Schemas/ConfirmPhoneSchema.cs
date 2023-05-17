using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class ConfirmPhoneSchema
{
    [Required]
    public string Phone { get; set; } = null!;
}
