using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] MovieFilterDto filter)
        {
            var movies = _movieService.GetAll(filter);
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var movie = _movieService.GetById(id);
            return movie is null ? NotFound("Film bulunamadı.") : Ok(movie);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] MovieDto request)
        {
            var response = _movieService.Create(request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id, [FromBody] MovieDto request)
        {
            var response = _movieService.Update(id, request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _movieService.Delete(id);
            return deleted ? Ok("Film başarıyla silindi") : NotFound("Film bulunamadı");
        }
        [HttpGet("{id}/details")]
        public IActionResult GetMovieWithInfo(Guid id, [FromQuery] PaginationFilter filter)
        {
            var result = _movieService.GetMovieWithInfo(id, filter);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}