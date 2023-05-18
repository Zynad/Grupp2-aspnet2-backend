using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetCoupon(string code)
    {
        if (ModelState.IsValid)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }
        return BadRequest();
    }

    [Authorize]
    [Route("DeleteCoupon")]
    [HttpPost]
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
