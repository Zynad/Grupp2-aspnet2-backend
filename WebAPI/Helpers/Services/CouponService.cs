using System.Collections;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class CouponService
{
    private readonly CouponRepo _couponRepo;

    public CouponService(CouponRepo couponRepo)
    {
        _couponRepo = couponRepo;
    }

    public async Task<CouponEntity> GetCouponByCodeAsync(string voucherCode)
    {

        var result = await _couponRepo.GetAsync(x => x.VoucherCode == voucherCode);
        if(result != null)
        {
            return result;
        }
        return null!;
        
    }


    public async Task<bool> DeleteAsync(string voucherCode)
    {

        try
        {
            var coupon = await _couponRepo.GetAsync(x => x.VoucherCode == voucherCode);
            if(coupon != null)
            {
                await _couponRepo.DeleteAsync(coupon);
                return true;
            }
        }
        catch { }

        return false;

    }

    public async Task<bool> AddAsync(CouponSchema schema)
    {
        var entity = schema;

        try
        {
            await _couponRepo.AddAsync(entity);
            return true;
        }
        catch { }
        
        return false;
    }

}




