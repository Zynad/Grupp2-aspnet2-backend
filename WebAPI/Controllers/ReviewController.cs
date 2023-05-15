using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers.Filters;
using WebAPI.Helpers.Services;
using WebAPI.Models.Schemas;

namespace WebAPI.Controllers
{
    [UseApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        
        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Route("AllReviews")]
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                return Ok(await _reviewService.GetAllAsync());
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [Route("GetByProductId")]
        [HttpGet]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            try
            {
                return Ok(await _reviewService.GetByProductId(productId));
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [Route("AddReview")]
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewSchema schema)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState not valid");
            try
            {
                return Ok(await _reviewService.CreateAsync(schema));
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}
