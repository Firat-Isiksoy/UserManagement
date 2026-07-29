using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
using UserManagement.Mappers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public List<CategoryDto> GetAll()
        {
            return _context.Categories.Select(c => c.ToDto()).ToList();
        }
        public CategoryDto GetById(Guid id)
        {
            return _context.Categories.Find(id)?.ToDto();
        }
        public ResponseModel<CategoryDto> Create(CategoryDto request)
        {
            var category = request.ToModel();
            category.Id = Guid.NewGuid();

           _context.Categories.Add(category);
           _context.SaveChanges(); 
         
            return new ResponseModel<CategoryDto>
            {
                Success = true,
                Error = null,
                Data = category.ToDto()
            };
        }
        public ResponseModel<CategoryDto> Update(Guid Id, CategoryDto request)
        {
            var existingCategory = _context.Categories.Find(Id);

            if (existingCategory is null)
            {
                return new ResponseModel<CategoryDto> { Success = false, Error = "Kategori bulunamadı", Data = null };
            }
            request.UpdateModel(existingCategory);
            _context.SaveChanges();

            return new ResponseModel<CategoryDto>
            {
                Success = true,
                Error = null,
                Data = existingCategory.ToDto()
            };
        }
        public bool Delete(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
