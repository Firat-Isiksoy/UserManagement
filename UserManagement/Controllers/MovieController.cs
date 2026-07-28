using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

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
        [HttpGet("category/{categoryId}")]
        public IActionResult GetMoviesByCategory(Guid categoryId)
        {      
            var movies = _movieService.GetMoviesByCategory(categoryId);

            if (!movies.Any())
            {
                return NotFound("Bu kategoriye ait herhangi bir film bulunamadı.");
            }
            var response = movies.Select(m => new MovieDto
            {
                Title = m.Title,
                Duration = m.Duration,
                AverageRating = m.AverageRating,
                ReleaseYear = m.ReleaseYear,
                Description = m.Description,
                CategoryId = m.CategoryId
            }).ToList();

            return Ok(new
            {
                Message = "Kategoriye ait filmler başarıyla listelendi",
                Data = response
            });
        }
        [HttpPost]
        public IActionResult Create(MovieDto request)
        {
            var movieModel = new MovieModel
            {
                Title = request.Title.Trim().ToLower(),
                Duration = request.Duration,
                AverageRating = request.AverageRating,
                ReleaseYear = request.ReleaseYear,
                Description = request.Description.Trim().ToLower(),
                CategoryId = request.CategoryId
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
                CategoryId =createdMovie.CategoryId,
            };
            return Ok(new
            {
                Message = "Film başarıyla eklendi",
                Data = response
            });
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, MovieDto request)
        {

            var movieModel = new MovieModel
            {
                Id = id,
                Title = request.Title.Trim().ToLower(),
                Duration = request.Duration,
                AverageRating = request.AverageRating,
                ReleaseYear = request.ReleaseYear,
                Description = request.Description?.Trim().ToLower(),
                CategoryId = request.CategoryId
            };

            var (success, error,updatedMovie) = _movieService.Update(id,movieModel);

            if (!success)
            {
                return BadRequest(error);
            }
            var response = new MovieDto
            {
                Title = updatedMovie.Title,
                Duration = updatedMovie.Duration,
                AverageRating = updatedMovie.AverageRating,
                ReleaseYear = updatedMovie.ReleaseYear,
                Description = updatedMovie.Description,
                CategoryId =updatedMovie.CategoryId,
            };
            return Ok(new
            {
                Message = "Film başarıyla güncellendi",
                Data = response
            });

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _movieService.Delete(id);
            return deleted ? Ok("Film başarıyla silindi") : NotFound("Film bulunamadı");
        }
    }
}

