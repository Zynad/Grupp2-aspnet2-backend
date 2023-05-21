using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
    public class UserCouponDTO
    {
        public string UserId { get; set; }
        public Guid CouponId { get; set; }

        public static implicit operator UserCouponDTO(UserCouponEntity entity)
        {
            return new UserCouponDTO
            {
                UserId = entity.UserId,
                CouponId = entity.CouponId
            };


        }
    }
}
