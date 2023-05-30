using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
	[UseApiKey]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		[Route("All")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetAllAsync();
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Get")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetByIdAsync(id);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No product found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Tag")]
		[HttpGet]
		public async Task<IActionResult> GetByTag([FromQuery]List<string> tags)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetByTagAsync(tags);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Category")]
		[HttpGet]
		public async Task<IActionResult> GetByCategory(string category)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetByCategoryAsync(category);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Sometihing went wrong, try again!");
		}

		[Route("SalesCategory")]
		[HttpGet]
		public async Task<IActionResult> GetBySalesCategory(string salescategory)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetBySalesCategoryAsync(salescategory);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Price")]
		[HttpGet]
		public async Task<IActionResult> GetByPrice(int minPrice, int maxPrice)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetByPriceAsync(minPrice, maxPrice);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Search")]
		[HttpGet]
		public async Task<IActionResult> GetByName(string name)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetByNameAsync(name);
				if (result != null)
					return Ok(result);
				else
					return NotFound("No products found");
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Filter")]
		[HttpPost]
		public async Task<IActionResult> GetFiltered(FilterSchema schema)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetFilteredProductsAsync(schema);
				if (result == null || !result.Any())
					return NotFound("No results found");

				return Ok(result);
			}
			
			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("Add")]
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddProduct(ProductSchema schema)
		{
			if (ModelState.IsValid)
			{
					var result = await _productService.CreateAsync(schema);
					if (result)
						return Created("", null);
			}
			
			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("Delete/{id}")]
		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			if (ModelState.IsValid)
			{
				var userName = HttpContext.User.Identity!.Name;
				if (userName != null)
				{
					var result = await _productService.DeleteAsync(id);
					if (result)
						return Ok("Product deleted");
				}
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Update")]
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateProduct(ProductSchema schema)
		{
			if (ModelState.IsValid)
			{
				var userName = HttpContext.User.Identity!.Name;
				if (userName != null)
				{
					var result = await _productService.UpdateAsync(schema);
					if (result)
						return Ok("Product updated");
				}
			}
			
			return BadRequest("Something went wrong, try again!");
		}
	}
}
