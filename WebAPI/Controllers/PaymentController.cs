using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers;

[Authorize]
[UseApiKey]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [Route("GetUserCreditCards")]
    public async Task<IActionResult> GetUserCreditCards()
    {
        if (ModelState.IsValid)
        {
            var result = await _paymentService.GetUserCreditCardsAsync(HttpContext.User.Identity!.Name!);
            if(result != null)
                return Ok(result);
        }
        return BadRequest("Something went wrong, try again!");
    }

    [HttpPost]
    [Route("RegisterCreditCard")]
    public async Task<IActionResult> RegisterCreditCard(RegisterCreditCardSchema schema)
    {
        if(ModelState.IsValid)
        {
            var result = await _paymentService.RegisterCreditCardsAsync(schema, HttpContext.User.Identity!.Name!);
            if(result)
                return Created("", null);
        }
        return BadRequest("Something went wrong, try again!");
    }

    [HttpDelete]
    [Route("RemoveCreditCard/{id}")]
    public async Task<IActionResult> RemoveCreditCard(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await _paymentService.DeleteCreditCardsAsync(id, HttpContext.User.Identity!.Name!);
            if(result)
                return Ok();
        }
        return BadRequest("Something went wrong, try again!");
    }
}
