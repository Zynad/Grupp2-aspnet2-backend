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
	public class CategoryController : ControllerBase
	{
		private readonly CategoryService _categoryService;

		public CategoryController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[Route("GetAllCategories")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await _categoryService.GetAllAsync());
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("GetCategoryById")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				return Ok(await _categoryService.GetByIdAsync(id));
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		//[Authorize(Roles = "Admin")]
		[Route("AddCategory")]
		[HttpPost]
		public async Task<IActionResult> AddCategory(CategorySchema schema)
		{
			if (ModelState.IsValid)
			{
				var category = await _categoryService.CreateAsync(schema);

				if (category != null)
				{
					return Created("", category);
				}
			}
			return BadRequest();

		}

		//[Authorize(Roles = "Admin")]
		[Route("DeleteCategory")]
		[HttpPost]
		public async Task<IActionResult> DeleteCategory(Guid id)
		{
			if (await _categoryService.DeleteAsync(id))
				return Ok();

			return BadRequest("Something went wrong");
		}

	}
}
