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
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
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
                var result = await _accountService.UpdateProfileAsync(schema);
                if(result != null)
                {
                    return Ok("Update is done");
                }
                
            }
            return BadRequest();
        }
    }
}
