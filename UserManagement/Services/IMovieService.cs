using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IMovieService
    {
    List<MovieDto> GetAll();
    List<MovieDto> GetMoviesByCategory(Guid categoryId);
    MovieDto? GetById(Guid id);
    ResponseModel<MovieDto> Create(MovieDto request);
    ResponseModel<MovieDto> Update(Guid Id,MovieDto movie);
    bool Delete(Guid id);
    }
}
