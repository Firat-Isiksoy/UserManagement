using UserManagement.DTOs;
using UserManagement.Models;
namespace UserManagement.Services
{
    public interface IUserService
    {
        List<UserModel> GetAll();
        UserModel? GetById(Guid id);
        ResponseModel<UserDto> Create(UserDto request);
        ResponseModel<UserDto> Update(Guid id, UserDto request);
        bool Delete(Guid id);
    }
}
