using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpGet]
        public IActionResult GetRatings([FromQuery] RatingFilterDto filter)
        {
            var result = _ratingService.GetRatings(filter);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create([FromBody] MovieRatingDto ratingDto)
        {
            var response = _ratingService.Create(ratingDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MovieRatingDto ratingDto)
        {
            var response = _ratingService.Update(id, ratingDto);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var isDeleted = _ratingService.Delete(id);
            if (!isDeleted)
            {
                return NotFound(new { Message = "Silinmek istenen değerlendirme bulunamadı." });
            }
            return Ok(new { Message = "Film değerlendirmesi başarıyla silindi." });
        }
    }
}