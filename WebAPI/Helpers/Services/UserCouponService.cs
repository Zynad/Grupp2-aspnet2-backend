using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;

namespace WebAPI.Helpers.Services
{
    public class UserCouponService : IUserCouponService
    {
        private readonly UserCouponRepo _userCouponRepo;
        private readonly IAccountService _accountService;
        private readonly ICouponService _couponService;
        public UserCouponService(UserCouponRepo userCouponRepo, IAccountService accountService, ICouponService couponService)
        {
            _userCouponRepo = userCouponRepo;
            _accountService = accountService;
            _couponService = couponService;
        }

        public async Task<UserCouponEntity> CheckDuplicateUserCouponAsync(string voucherCode, string userName)
        {
            try
            {
                var userId = await _accountService.GetUserIdAsync(userName);
                var coupon = await _couponService.GetCouponByCodeAsync(voucherCode);
                if (userId != null && coupon != null)
                {
                    var userCoupon = new UserCouponEntity { UserId = userId, CouponId = coupon!.Id };

                    var result = await _userCouponRepo.GetAsync(x => x.UserId == userCoupon.UserId && x.CouponId == userCoupon.CouponId);
                    if (result == null)
                    {
                        return userCoupon;
                    }
                }
            }
            catch { }

            return null!;
        }
        public async Task<UserCouponDTO> AddUserCouponAsync(UserCouponEntity entity)
        {
            try
            {
                var result = await _userCouponRepo.AddAsync(entity);
                if (result != null)
                {
                    var dto = await ConvertToDto(result);
                    return dto;
                }
            }
            catch { }
            return null!;
        }
        public async Task<IEnumerable<UserCouponDTO>> GetAllUsedUserCouponsAsync(string userName)
        {
            var userCoupons = new List<UserCouponDTO>();
            try
            {
                var userId = await _accountService.GetUserIdAsync(userName);
                if (userId != null)
                {
                    var result = await _userCouponRepo.GetListAsync(x => x.UserId == userId && x.Used == true);
                    foreach (var coupon in result)
                    {
                        var dto = await ConvertToDto(coupon);
                        userCoupons.Add(dto);
                    }

                    return userCoupons;
                }
            }
            catch { }

            return null!;
        }
        public async Task<IEnumerable<UserCouponDTO>> GetAllUnusedUserCouponsAsync(string userName)
        {
            var userCoupons = new List<UserCouponDTO>();
            try
            {
                var userId = await _accountService.GetUserIdAsync(userName);
                if (userId != null)
                {
                    var result = await _userCouponRepo.GetListAsync(x => x.UserId == userId && x.Used == false);
                    foreach (var coupon in result)
                    {
                        var dto = await ConvertToDto(coupon);
                        userCoupons.Add(dto);
                    }

                    return userCoupons;
                }
            }
            catch { }

            return null!;
        }
        public async Task<IEnumerable<UserCouponDTO>> GetAllUserCouponsAsync(string userName)
        {
            var userCoupons = new List<UserCouponDTO>();
            try
            {
                var userId = await _accountService.GetUserIdAsync(userName);
                if (userId != null)
                {
                    var result = await _userCouponRepo.GetListAsync(x => x.UserId == userId);
                    foreach (var coupon in result)
                    {
                        var dto = await ConvertToDto(coupon);
                        userCoupons.Add(dto);
                    }

                    return userCoupons;
                }
            }
            catch { }

            return null!;
        }
        private async Task<UserCouponDTO> ConvertToDto(UserCouponEntity userCoupon)
        {
            var dto = new UserCouponDTO();
            dto.UserId = userCoupon.UserId;
            dto.Used = userCoupon.Used;
            dto.Coupon = await _couponService.GetCouponByIDAsync(userCoupon.CouponId);

            return dto;
        }
    }
}
