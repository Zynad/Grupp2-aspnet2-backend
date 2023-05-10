using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Repositories;
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
			try
			{
				return Ok(await _productService.GetAllAsync());
			}
			catch 
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("Get")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				return Ok(await _productService.GetByIdAsync(id));
			}
			catch 
			{
				return BadRequest("Something went wrong");			
			}
		}

		[Route("Tag")]
		[HttpGet]
		public async Task<IActionResult> GetByTag([FromQuery]List<string> tags)
		{
			try
			{
				return Ok(await _productService.GetByTagAsync(tags));
			}
			catch 
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("Category")]
		[HttpGet]
		public async Task<IActionResult> GetByCategory(string category)
		{
			try
			{
				return Ok(await _productService.GetByCategoryAsync(category));
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("SalesCategory")]
		[HttpGet]
		public async Task<IActionResult> GetBySalesCategory(string salescategory)
		{
			try
			{
				return Ok(await _productService.GetBySalesCategoryAsync(salescategory));
			}
			catch 
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("Price")]
		[HttpGet]
		public async Task<IActionResult> GetByPrice(int minPrice, int maxPrice)
		{
			try
			{
				return Ok(await _productService.GetByPriceAsync(minPrice, maxPrice));
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("Search")]
		[HttpGet]
		public async Task<IActionResult> GetByName(string name)
		{
			try
			{
				return Ok(await _productService.GetByNameAsync(name));
			}
			catch 
			{
				return BadRequest("Something went wrong");
			}
		}

		//[Authorize(Roles = "Admin, ProductManager")]
		[Route("Add")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(ProductSchema schema)
		{
			if (ModelState.IsValid)
			{
				var product = await _productService.CreateAsync(schema);
        
				if (product != null)
				{
					return Created("", product);
				}
			}
			return BadRequest();

		}

		//[Authorize(Roles = "Admin, ProductManager")]
		[Route("Delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			if (await _productService.DeleteAsync(id))
				return Ok();

			return BadRequest("Something went wrong");
		}
	}
}
