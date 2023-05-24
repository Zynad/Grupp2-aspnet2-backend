using WebAPI.Helpers.Repositories;

namespace WebAPI.Helpers.Services.Coupon
{
    public class UserCouponService
    {
        private readonly UserCouponRepo _userCouponRepo;

        public UserCouponService(UserCouponRepo userCouponRepo)
        {
            _userCouponRepo = userCouponRepo;
        }

  
    }
}
