using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
           try
           {
                var movies = _movieService.GetAll(filter);
                return Ok(movies);
           }
           catch (Exception ex)
           {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var movie = _movieService.GetById(id);
            return movie is null ? NotFound("Film bulunamadı.") : Ok(movie);
        }       
        [HttpPost]
        public IActionResult Create(MovieDto request)
        {          
            var response = _movieService.Create(request);

            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new {Message = "Film başarıyla eklendi", response.Data});        
        }
        [HttpPost("ratings")]
        public IActionResult AddRating(MovieRatingDto rating)
        {
            var response = _movieService.AddRating(rating);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new { Message = "Film değerlendirmesi başarıyla eklendi", response.Data });
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, MovieDto request)
        {
            var response = _movieService.Update(id,request);

            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new{ Message = "Film başarıyla güncellendi", response.Data });
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _movieService.Delete(id);
            return deleted ? Ok("Film başarıyla silindi") : NotFound("Film bulunamadı");
        }
    }
}

