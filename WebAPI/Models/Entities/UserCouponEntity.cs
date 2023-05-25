using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Dtos;

namespace WebAPI.Models.Entities
{
    public class UserCouponEntity
    {
        [Key]
        public string UserId { get; set; } = null!;
        public Guid CouponId { get; set; }
        public bool Used { get; set; } = false;
        public string PartitionKey { get; set; } = "UserCoupon";
    }
}
