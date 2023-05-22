using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
	//[UseApiKey]
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
			if (ModelState.IsValid)
			{
				var result = await _productService.GetAllAsync();
				if (result != null)
					return Ok(result);
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
			}
			
			return BadRequest("Something went wrong, try again!");
		}

		[Route("Filter")]
		[HttpGet]
		public async Task<IActionResult> GetFiltered(string? name, int? minPrice, int? maxPrice, string? tags, string? category, string? salesCategory, int? amount)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.GetFilteredProductsAsync(name, minPrice, maxPrice, tags, category, salesCategory, amount);
				if (result != null)
					return Ok(result);
			}
			
			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("Add")]
		[HttpPost]
		public async Task<IActionResult> AddProduct(ProductSchema schema)
		{
			if (ModelState.IsValid)
			{
				var product = await _productService.CreateAsync(schema);
        
				if (product)
				{
					var result = await _productService.CreateAsync(schema);
					if (result)
						return Created("", null);
				}
			}
			
			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("Delete")]
		[HttpPost]
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
