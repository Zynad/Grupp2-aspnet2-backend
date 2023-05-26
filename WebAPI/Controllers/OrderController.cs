using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

		public OrderController(ProductService productService, OrderService orderService)
		{
			_productService = productService;
			_orderService = orderService;
		}

        [Route("AllOrders")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            if(ModelState.IsValid)
            {
                var result = await _orderService.GetAllOrdersAsync();
                if(result != null)
                    return Ok(result);
            }

            return BadRequest("Something went wrong, try again!");
        }

        [Route("CreateOrder")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderSchema schema)
        {
            if(ModelState.IsValid)
            {
                var userEmail = HttpContext.User.Identity!.Name;
                if(userEmail != null)
                {
					var result = await _orderService.CreateOrderAsync(schema, userEmail);
                    if (result)
                        return Ok("Order created");
				}
            }

            return BadRequest("Something went wrong, try again!");
        }
	}
}
