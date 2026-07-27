using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.DTOs;

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
        public IActionResult GetAllMovies() => Ok(_movieService.GetAll());
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var movie = _movieService.GetById(id);
            return movie is null ? NotFound("Film bulunamadı.") : Ok(movie);
        }
        [HttpPost]
        public IActionResult Create(MovieDto request)
        {
            var movieModel = new MovieModel
            {
                Title = request.Title,
                Duration = request.Duration,
                AverageRating = request.AverageRating,
                ReleaseYear = request.ReleaseYear,
                Description = request.Description,
            };

            var (success, error, createdMovie) = _movieService.Create(movieModel);

            if (!success)
            {
                return BadRequest(error);
            }
            var response = new MovieDto
            {
                Title = createdMovie.Title,
                Duration = createdMovie.Duration,
                AverageRating = createdMovie.AverageRating,
                ReleaseYear = createdMovie.ReleaseYear,
                Description = createdMovie.Description,
            };
            return Ok(new
            {
                Message = "Film başarıyla eklendi",
                Data = response
            });
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, MovieModel updatedMovie)
        {
            var (success, error) = _movieService.Update(id, updatedMovie);
            return success ? Ok("Film başarıyla güncellendi") : NotFound(error);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _movieService.Delete(id);
            return deleted ? Ok("Film başarıyla silindi") : NotFound("Film bulunamadı");
        }
    }
}

