using UserManagement.Models;

namespace UserManagement.Services
{
    public interface ICategoryService
    {
    List<CategoryModel> GetAll();
    CategoryModel? GetById(Guid id);
    (bool Success, string Error) Create(CategoryModel category);
    (bool Success, string Error) Update(Guid Id, CategoryModel category);
    bool Delete(Guid id);
    }
}
