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

        public (bool Success, string Error, MovieModel? Movie) Create(MovieModel movie)
        {
            movie.Id = Guid.NewGuid();
            movie.Title = movie.Title.Trim();
            movie.CreatedAt = DateTime.UtcNow;

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return (true,string.Empty,movie);
        }

        public List<MovieModel> GetAll() => _context.Movies.ToList();
        public List<MovieModel> GetMoviesByCategory(Guid categoryId)
        {
            return _context.Movies.Where(m => m.CategoryId == categoryId).ToList();
        }
        public MovieModel? GetById(Guid id) => _context.Movies.Find(id);
        public (bool Success, string Error, MovieModel? Movie) Update(Guid Id, MovieModel movie)
        {
            var existingMovie = _context.Movies.Find(movie.Id);
            if (existingMovie is null) return (false,"Aranan film bulunamadı",null);
            existingMovie.Title = movie.Title.Trim();
            existingMovie.Description = movie.Description?.Trim();
            existingMovie.CategoryId = movie.CategoryId;
            existingMovie.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return (true, "Film başarıyla güncellendi",existingMovie);
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
