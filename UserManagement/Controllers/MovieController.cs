using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public IActionResult Create(MovieModel movieModel)
        {
            var (success, error) = _movieService.Create(movieModel);
            return success ? Ok("Film başarıyla eklendi") : BadRequest(error);
        }
    }
}

