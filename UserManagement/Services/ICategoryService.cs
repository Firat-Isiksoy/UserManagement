using UserManagement.Models;
using UserManagement.DTOs;

namespace UserManagement.Services
{
    public interface ICategoryService
    {
    List<CategoryDto> GetAll();
    CategoryDto GetById(Guid id);
    ResponseModel<CategoryDto> Create(CategoryDto category);
    ResponseModel<CategoryDto> Update(Guid Id, CategoryDto category);
    bool Delete(Guid id);
    }
}
