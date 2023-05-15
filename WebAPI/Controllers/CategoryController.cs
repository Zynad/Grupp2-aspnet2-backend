﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
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
			if (ModelState.IsValid)
			{
				var result = await _categoryService.GetAllAsync();
				if (result != null)
					return Ok(result);
			}

			return BadRequest("Something went wrong, try again");
		}

		[Route("GetCategoryById")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			if (ModelState.IsValid)
			{
				var result = await _categoryService.GetByIdAsync(id);
				if (result != null)
					return Ok(result);
			}

			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("AddCategory")]
		[HttpPost]
		public async Task<IActionResult> AddCategory(CategorySchema schema)
		{
			if (ModelState.IsValid)
			{
				var userName = HttpContext.User.Identity!.Name;
				if (userName != null)
				{
					var result = await _categoryService.CreateAsync(schema);
					if (result)
						return Created("", null);
				}
			}

			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("DeleteCategory")]
		[HttpPost]
		public async Task<IActionResult> DeleteCategory(Guid id)
		{
			if (ModelState.IsValid)
			{
				var userName = HttpContext.User.Identity!.Name;
				if (userName != null)
				{
					var result = await _categoryService.DeleteAsync(id);
					if (result)
						return Ok("Category deleted");
				}
			}

			return BadRequest("Something went wrong, try again!");
		}

	}
}
