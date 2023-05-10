using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
    [UseApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(AccountService accountService, UserManager<IdentityUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountSchema schema)
        {
            if(ModelState.IsValid)
            {
                if(await _accountService.RegisterAsync(schema))
                {
                    return Created("", null);
                }
            }
            return BadRequest("Something went wrong, try again!");
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginAccountSchema schema)
        {
            if(ModelState.IsValid)
            {
                var token = await _accountService.LogInAsync(schema);
                if(!string.IsNullOrEmpty(token))
                    return Ok(token);
            }
            return BadRequest("Incorrect email or password");
        }
        [Authorize]
        [Route("LogOut")]
        [HttpPost]
        public async Task<IActionResult> LogOutAsync()
        {
            await _accountService.LogOutAsync();
            return Ok();
        }

        [Authorize]
        [Route("UpdateProfile")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfileAsync(UpdateUserSchema schema)
        {
            
            if (ModelState.IsValid)
            {
                var userName = HttpContext.User.Identity.Name;
                var result = await _accountService.UpdateProfileAsync(schema,userName!);
                if(result != null)
                {                   
                    return Ok("Update is done");
                }
                return BadRequest("Model valid, something else is wrong");
            }
            return BadRequest("Model not valid");
        }
        [Authorize]
        [Route("GetProfile")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {

            if (ModelState.IsValid)
            {
                var userName = HttpContext.User.Identity.Name;
                var result = await _accountService.GetProfile(userName!);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("Model valid, something else is wrong");
            }
            return BadRequest("Model not valid");
        }
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                if (await _accountService.ResetPassword(email))
                {
                    return Ok("An email has been sent");
                }
                return StatusCode(500, "Something went wrong on the server");
            }
            return BadRequest("You must enter an email");
        }

        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    return Ok("Email confirmed");
                }
            }
            return StatusCode(500, "Something went wrong on the server");
        }
        
    }
}
