using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IUserService
    {
        List<UserModel> GetAll();
        UserModel? GetById(Guid id);
        (bool Success, string Error, UserModel? User) Create(UserModel user);
        (bool Success, string Error, UserModel? User) Update(Guid id, UserModel updatedUser);
        bool Delete(Guid id);
    }
}
