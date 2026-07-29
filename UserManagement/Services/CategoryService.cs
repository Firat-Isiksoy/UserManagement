using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
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
        public List<CategoryModel> GetAll()
        {
            return _context.Categories.ToList();
        }
        public CategoryModel? GetById(Guid id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }
        public ResponseModel<CategoryDto> Create(CategoryDto request)
        {
            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim()
            };

           _context.Categories.Add(category);
           _context.SaveChanges();
           var responseDto = new CategoryDto
           {
                Name = category.Name
           };
            return new ResponseModel<CategoryDto>
            {
                Success = true,
                Error = null,
                Data = responseDto
            };
        }
        public ResponseModel<CategoryDto> Update(Guid Id, CategoryDto request)
        {
           var existingCategory = _context.Categories.Find(Id);
            if (existingCategory == null)
            {
                return new ResponseModel<CategoryDto>
                {
                    Success = false,
                    Error = "Aranan kategori bulunamadı",
                    Data = null
                };                
            }
            existingCategory.Name = request.Name.Trim();

            var responseDto = new CategoryDto
            {
                Name = existingCategory.Name
            };
            _context.SaveChanges();
            return new ResponseModel<CategoryDto>
            {
                Success = true,
                Error = null,
                Data = responseDto
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
