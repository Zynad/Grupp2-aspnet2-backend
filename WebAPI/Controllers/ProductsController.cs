using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
	[UseApiKey]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ProductService _productService;

		public ProductsController(ProductService productService)
		{
			_productService = productService;
		}

		[Route("All")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _productService.GetAllAsync());
		}

		[Route("Get")]
		[HttpGet]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(await _productService.GetByIdAsync(id));
		}

		[Route("Tag")]
		[HttpGet]
		public async Task<IActionResult> GetByTag(string tag)
		{
			return Ok(await _productService.GetByTagAsync(tag));
		}

		[Route("Search")]
		[HttpGet]
		public async Task<IActionResult> GetByName(string name)
		{
			return Ok(await _productService.GetByNameAsync(name));
		}

		//[Authorize(Roles = "Admin, ProductManager")]
		[Route("Add")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(ProductSchema schema)
		{
			if (await _productService.CreateAsync(schema))
				return Created("", null);

			return BadRequest();
		}

		//[Authorize(Roles = "Admin, ProductManager")]
		[Route("Delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			if (await _productService.DeleteAsync(id))
				return Ok();

			return BadRequest();
		}
	}
}
