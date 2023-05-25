using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers;

[UseApiKey]
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost]
    [Route("RegisterAddress")]
    public async Task<IActionResult> RegisterAddress(RegisterAddressSchema schema)
    {
        if (ModelState.IsValid)
        {
            var userName = HttpContext.User.Identity!.Name;
            if(userName != null)
            {
                if (await _addressService.RegisterAddressAsync(schema, userName!))
                {
                    return Created("", null);
                }
            }
                return BadRequest("Something went wrong, try again!");
        }
        return BadRequest("Something went wrong, try again!");
    }

    [HttpPut]
    [Route("UpdateAddress")]
    public async Task<IActionResult> UpdateAddress(UpdateAddressSchema schema)
    {
        if (ModelState.IsValid)
        {
            var userName = HttpContext.User.Identity!.Name;
            if (userName != null)
            {
                var result = await _addressService.UpdateAddressAsync(schema, userName);
                if (result != null)
                {
                    return Ok("Address updated");
                }
            }

        }
        return BadRequest("Something went wrong, try again!");
    }

    [HttpGet]
    [Route("GetUserAddresses")]
    public async Task<IActionResult> GetUserAddresses()
    {
        if (ModelState.IsValid)
        {
            var userName = HttpContext.User.Identity!.Name;
            if (userName != null)
            {
                var result = await _addressService.GetUserAddressesAsync(userName);
                if (result != null)
                    return Ok(result);
            }
        }
        return BadRequest("Something went wrong, try again!");
    }

    [HttpDelete]
    [Route("RemoveAddress/{id}")]
    public async Task<IActionResult> RemoveAddress(int id)
    {
        if(ModelState.IsValid)
        {
            var result = await _addressService.DeleteAddressAsync(id);
            if (result)
                return Ok("Address removed");
        }
        return BadRequest("Something went wrong, try again!");
    }
}
