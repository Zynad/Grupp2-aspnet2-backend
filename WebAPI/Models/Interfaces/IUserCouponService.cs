using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Interfaces;

public interface IUserCouponService
{
    Task<UserCouponDTO> AddUserCouponAsync(UserCouponEntity entity);
    Task<UserCouponEntity> CheckDuplicateUserCouponAsync(string voucherCode, string userName);
    Task<IEnumerable<UserCouponDTO>> GetAllUnusedUserCouponsAsync(string userName);
    Task<IEnumerable<UserCouponDTO>> GetAllUsedUserCouponsAsync(string userName);
    Task<IEnumerable<UserCouponDTO>> GetAllUserCouponsAsync(string userName);
}