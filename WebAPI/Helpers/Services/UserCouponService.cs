using WebAPI.Helpers.Repositories;

namespace WebAPI.Helpers.Services
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
