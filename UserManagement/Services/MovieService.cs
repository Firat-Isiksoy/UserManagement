using UserManagement.DTOs;
using UserManagement.Mappers;
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
            var newMovie = request.ToModel();
            newMovie.Id = Guid.NewGuid();
            newMovie.CreatedAt = DateTime.UtcNow;

            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            return new ResponseModel<MovieDto>
            {
                Success = true,
                Error = null,
                Data = newMovie.ToDto()
            };
        }
        public List<MovieDto> GetAll() => _context.Movies.Select(m => m.ToDto()).ToList();
        public List<MovieDto> GetMoviesByCategory(Guid categoryId)
        {
            return _context.Movies
                   .Where(m => m.CategoryId == categoryId)
                   .Select(m => m.ToDto())
                   .ToList();
        }
        public MovieDto? GetById(Guid id) => _context.Movies.Find(id)?.ToDto();
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
            movie.UpdateModel(existingMovie);
            existingMovie.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            
           return new ResponseModel<MovieDto>
           {
                Success = true,
                Error = null,
                Data = existingMovie.ToDto()
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
