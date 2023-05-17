using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Email;
using WebAPI.Models.Interfaces;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly MailService _mailService;
    private readonly UserManager<IdentityUser> _userManager;

    public MailController(MailService mailService, UserManager<IdentityUser> userManager)
    {
        _mailService = mailService;
        _userManager = userManager;
    }
    [UseApiKey]
    [Route("Sendmail")]
    [HttpPost]
    public async Task<IActionResult> SendMailAsync(MailData mailData)
    {
        bool result = await _mailService.SendAsync(mailData, new CancellationToken());

        if (result)
        {
            return Ok("Mail has successfully been sent.");
        }
        else
        {
            return Problem("An error occured. The Mail could not be sent.");
        }
    }
    [Route("ConfirmEmail")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed");
            }
        }
        return Problem("Something went wrong on the server");
    }
}
