using UserManagement.DTOs;
using UserManagement.Models;
namespace UserManagement.Services
{
    public interface IUserService
    {
        List<UserDto> GetAll();
        UserDetailsDto? GetById(Guid id);
        ResponseModel<UserDetailsDto> Create(UserCreateDto request);
        ResponseModel<UserDetailsDto> Update(Guid id, UserCreateDto request);
        bool Delete(Guid id);
    }
}
