using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
    [UseApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class UserCouponController : ControllerBase
    {
        private readonly UserCouponService _userCouponService;

        public UserCouponController(UserCouponService userCouponService)
        {
            _userCouponService = userCouponService;
        }

        [Authorize]
        [Route("AddUserCoupon")]
        [HttpPost]
        public async Task<IActionResult> AddUserCoupon(UserCouponSchema schema)
        {
            var userName = HttpContext.User.Identity!.Name;
            if (ModelState.IsValid)
            {
                var entity = await _userCouponService.CheckDuplicateUserCouponAsync(schema.VoucherCode, userName!);
                if (entity != null)
                {
                    var result = await _userCouponService.AddUserCouponAsync(entity);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return Conflict();
                
            }
            return BadRequest();
        }
        [Authorize]
        [Route("GetAllUnused")]
        [HttpGet]
        public async Task<IActionResult> GetAllUnused()
        {
            var username = HttpContext.User.Identity!.Name;
            var result = await _userCouponService.GetAllUnusedUserCouponsAsync(username!);
            if(result != null)
            {
                return Ok(result);
            }
            return Problem();
        }
        [Authorize]
        [Route("GetAllUsed")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsed()
        {
            var username = HttpContext.User.Identity!.Name;
            var result = await _userCouponService.GetAllUsedUserCouponsAsync(username!);
            if (result != null)
            {
                return Ok(result);
            }
            return Problem();
        }
        [Authorize]
        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var username = HttpContext.User.Identity!.Name;
            var result = await _userCouponService.GetAllUserCouponsAsync(username!);
            if (result != null)
            {
                return Ok(result);
            }
            return Problem();
        }
    }
}
