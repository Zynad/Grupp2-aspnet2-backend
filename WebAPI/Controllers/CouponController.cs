using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.Extensions.Azure;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers;

[UseApiKey]
[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{

    private readonly CouponService _couponService;

    public CouponController(CouponService couponService)
    {
        _couponService = couponService;
    }
    [Route("GetCoupon")]
    [HttpGet]
    public async Task<IActionResult> GetCoupon(string voucher)
    {
        if (ModelState.IsValid)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(voucher);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }
        return BadRequest();
    }

    [Route("GetAllCoupons")]
    [HttpGet]
    public async Task<IActionResult> GetAllCoupons()
    {
        
        var result = await _couponService.GetAllAsync();
        if (result != null)
        {
            return Ok(result);
        }
        return Problem();

    }

    [Authorize]
    [Route("DeleteCoupon/{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCoupon(string code)
    {
        if (ModelState.IsValid)
        {
            if (await _couponService.DeleteAsync(code))
                return Ok();
        }
        return BadRequest();
    }
    [Authorize]
    [Route("AddCoupon")]
    [HttpPost]
    public async Task<IActionResult> AddCoupon(CouponSchema schema)
    {
        if (ModelState.IsValid)
        {
            if (await _couponService.AddAsync(schema))
                return Ok();
        }
        return BadRequest();
    }
}
