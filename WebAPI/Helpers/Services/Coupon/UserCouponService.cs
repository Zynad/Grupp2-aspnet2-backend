using WebAPI.Helpers.Repositories;
using WebAPI.Models.Entities;
using System.Collections;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;
using WebAPI.Helpers.Repositories.BaseModels;

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
