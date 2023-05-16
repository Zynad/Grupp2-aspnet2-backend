using System.Collections;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services
{
    public class CouponService
    {
        private readonly CouponRepo _couponRepo;

        public CouponService(CouponRepo couponRepo)
        {
            _couponRepo = couponRepo;
        }

        public object CouponDTO { get; internal set; }

        public async Task<CouponEntity> GetCouponByCodeAsync(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            var result= await _couponRepo.GetAsync(x => x.Code == code);
            return result;
        }


        public async Task<bool> DeleteAsync(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            try
            {
                var coupon = await _couponRepo.GetAsync(x => x.Code == code);
                await _couponRepo.DeleteAsync(coupon);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

            
        }

        public async Task<bool> AddAsync(CouponSchema schema)
        {
            CouponEntity entity = schema;

            try
            {
                await _couponRepo.AddAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }



    
}
