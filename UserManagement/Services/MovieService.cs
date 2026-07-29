using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class MovieService : IMovieService
    {

        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public ResponseModel<MovieDto> Create(MovieDto request)
        {
           var movie = new MovieModel
           {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Duration = request.Duration,
                AverageRating = request.AverageRating,
                ReleaseYear = request.ReleaseYear,
                Description = request.Description?.Trim(),
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
           };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            var responseDto = new MovieDto
            {
                Title = movie.Title,
                Duration = movie.Duration,
                AverageRating = movie.AverageRating,
                ReleaseYear = movie.ReleaseYear,
                Description = movie.Description,
                CategoryId = movie.CategoryId
            };
            return new ResponseModel<MovieDto>
            {
                Success = true,
                Error = null,
                Data = responseDto
            };
        }
        public List<MovieModel> GetAll() => _context.Movies.ToList();
        public List<MovieModel> GetMoviesByCategory(Guid categoryId)
        {
            return _context.Movies.Where(m => m.CategoryId == categoryId).ToList();
        }
        public MovieModel? GetById(Guid id) => _context.Movies.Find(id);
        public ResponseModel<MovieDto> Update(Guid Id, MovieDto movie)
        {
            var existingMovie = _context.Movies.Find(Id);
            if (existingMovie is null)                
            {
                return new ResponseModel<MovieDto>
                {
                    Success = false,
                    Error = "Aranan film bulunamadı",
                    Data = null
                };              
            };

            existingMovie.Title = movie.Title.Trim();
            existingMovie.Duration = movie.Duration;
            existingMovie.AverageRating = movie.AverageRating;
            existingMovie.ReleaseYear = movie.ReleaseYear;
            existingMovie.Description = movie.Description?.Trim();
            existingMovie.CategoryId = movie.CategoryId;
            existingMovie.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            
            var updatedMovieDto = new MovieDto
            {
                Title = existingMovie.Title,
                Duration = existingMovie.Duration,
                AverageRating = existingMovie.AverageRating,
                ReleaseYear = existingMovie.ReleaseYear,
                Description = existingMovie.Description,
                CategoryId = existingMovie.CategoryId
            };
           return new ResponseModel<MovieDto>
            {
                Success = true,
                Error = null,
                Data = updatedMovieDto
            };
        }
        public bool Delete(Guid id)
        {
            var movie = _context.Movies.Find(id);
            if (movie is null) return false;
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }
    }
}
