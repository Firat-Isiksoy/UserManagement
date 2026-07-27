using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IMovieService
    {
    List<MovieModel> GetAll();
    List<MovieModel> GetByCategoryId(Guid id);
    MovieModel? GetById(Guid id);
    (bool Success, string Error) Create(MovieModel movie);
    (bool Success, string Error) Update(Guid Id,MovieModel movie);
    bool Delete(Guid id);
    }
}
