using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
    public class UserCouponDTO
    {
        public string UserId { get; set; } = null!;
        public CouponDTO Coupon { get; set; } = null!;
        public bool Used { get; set; }
    }
}
