using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IMovieService
    {
    List<MovieModel> GetAll();
    List<MovieModel> GetMoviesByCategory(Guid categoryId);
    MovieModel? GetById(Guid id);
    ResponseModel<MovieDto> Create(MovieDto request);
    ResponseModel<MovieDto> Update(Guid Id,MovieDto movie);
    bool Delete(Guid id);
    }
}
