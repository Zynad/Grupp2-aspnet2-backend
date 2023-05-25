using Microsoft.Build.Framework;

namespace WebAPI.Models.Schemas;

public class UserCouponSchema
{
    [Required]
    public string VoucherCode { get; set; } = null!;
}
