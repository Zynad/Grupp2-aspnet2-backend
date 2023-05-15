using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Twitter;
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

		#region Standard stuff

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
        #endregion


        #region External Login

        public async Task Facebook() => await HttpContext.ChallengeAsync(
            FacebookDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = Url.Action("ExternalAuthentication") }
        );

		public async Task Google() => await HttpContext.ChallengeAsync(
			GoogleDefaults.AuthenticationScheme,
			new AuthenticationProperties { RedirectUri = Url.Action("ExternalAuthentication") }
		);

		public async Task Twitter() => await HttpContext.ChallengeAsync(
			TwitterDefaults.AuthenticationScheme,
			new AuthenticationProperties { RedirectUri = Url.Action("ExternalAuthentication") }
		);

        public async Task<IActionResult> ExternalAuthentication()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Type,
                    claim.Value,
                    claim.Issuer,
                    claim.Subject,
                    claim.Properties
                });

                return Ok(claims);
            }

            ModelState.AddModelError("", "Unable to login");
            return RedirectToAction("Index");
        }

		#endregion
	}
}
