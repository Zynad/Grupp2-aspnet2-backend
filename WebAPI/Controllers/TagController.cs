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
	public class TagController : ControllerBase
	{
		private readonly TagService _tagService;

		public TagController(TagService tagService)
		{
			_tagService = tagService;
		}

		[Route("GetAllTags")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				return Ok(await _tagService.GetAllAsync());
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		[Route("GetTagById")]
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			try
			{
				return Ok(await _tagService.GetByIdAsync(id));
			}
			catch
			{
				return BadRequest("Something went wrong");
			}
		}

		//[Authorize(Roles = "Admin")]
		[Route("AddTag")]
		[HttpPost]
		public async Task<IActionResult> AddTag(TagSchema schema)
		{
			if (ModelState.IsValid)
			{
				var tag = await _tagService.CreateAsync(schema);

				if (tag)
				{
					return Created("", tag);
				}
			}
			return BadRequest();
		}

		//[Authorize(Roles = "Admin")]
		[Route("DeleteTag")]
		[HttpPost]
		public async Task<IActionResult> DeleteTag(Guid id)
		{
			if (await _tagService.DeleteAsync(id))
				return Ok();

			return BadRequest("Something went wrong");
		}
	}
}
