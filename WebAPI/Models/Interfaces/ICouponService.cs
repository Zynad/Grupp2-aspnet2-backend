using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface ICouponService
    {
        Task<bool> AddAsync(CouponSchema schema);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CouponDTO>> GetAllAsync();
        Task<CouponEntity> GetCouponByCodeAsync(string voucherCode);
        Task<CouponEntity> GetCouponByIDAsync(Guid id);
    }
}