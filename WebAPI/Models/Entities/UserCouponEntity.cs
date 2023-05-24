using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class UserCouponEntity
    {
        [Key]
        public string UserId { get; set; } = null!;
        public Guid CouponId { get; set; }
        public string PartitionKey { get; set; } = "UserCoupon";
    }
}
