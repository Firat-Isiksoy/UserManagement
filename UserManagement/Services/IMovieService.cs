using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IMovieService
    {
    List<MovieModel> GetAll();
    List<MovieModel> GetByCategoryId(Guid id);
    MovieModel? GetById(Guid id);
    (bool Success, string Error, MovieModel? Movie) Create(MovieModel movie);
    (bool Success, string Error, MovieModel? Movie) Update(Guid Id,MovieModel movie);
    bool Delete(Guid id);
    }
}
