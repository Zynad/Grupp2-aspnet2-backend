using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Sms;

namespace WebAPI.Controllers;

//[UseApiKey]
[Route("api/[controller]")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly SmsService _service;

    public SmsController(SmsService service)
    {
        _service = service;
    }

    [Route("SendSms")]
    [HttpPost]
    public async Task<IActionResult> SendSms(SendSmsSchema schema)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.SendSms(schema);
            if (result)
            {
                return Ok();
            }
            return StatusCode(500, "Something went wrong on the server");
        }
        return BadRequest();

    }
}
