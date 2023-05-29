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
	public class TagController : ControllerBase
	{
		private readonly ITagService _tagService;

		public TagController(ITagService tagService)
		{
			_tagService = tagService;
		}

		[Route("GetAllTags")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (ModelState.IsValid)
			{
				var result = await _tagService.GetAllAsync();
				if (result != null)
					return Ok(result);
			}

			return BadRequest("Something went wrong, try again!");
		}

		[Route("GetTagById")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			if (ModelState.IsValid)
			{
				var result = await _tagService.GetByIdAsync(id);
				if (result != null)
					return Ok(result);
			}

			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("AddTag")]
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddTag(TagSchema schema)
		{
			if (ModelState.IsValid)
			{
					var result = await _tagService.CreateAsync(schema);
					if (result)
						return Created("", null);
			}
			return BadRequest("Something went wrong, try again!");
		}
		
		[Route("DeleteTag")]
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteTag(Guid id)
		{
			if (ModelState.IsValid)
			{
				var userName = HttpContext.User.Identity!.Name;
				if (userName != null)
				{
					var result = await _tagService.DeleteAsync(id);
					if (result)
						return Ok("Tag deleted");
				}
			}
			
			return BadRequest("Something went wrong, try again!");
		}
	}
}
