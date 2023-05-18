using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
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





        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> GetCoupon(string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }


        [Route("All")]
        [HttpGet]
        public async Task<IActionResult> GetCoupons()
        {
            try
            {
                return Ok(await _couponService.GetAllAsync());
            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }

        }


        [Authorize(Roles = "Admin, ProductManager")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(string code)
        {
            if (await _couponService.DeleteAsync(code))
                return Ok();

            return BadRequest();
        }



        //[Authorize(Roles = "Admin, ProductManager")]
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> AddCoupon(CouponSchema schema)
        {
            if (await _couponService.AddAsync(schema))
                return Ok();

            return BadRequest();
        }



        



    }

    
}
