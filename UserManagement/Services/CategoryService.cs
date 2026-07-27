using Microsoft.EntityFrameworkCore;
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
            return _context.Categories.Include(c => c.Movies).ToList();
        }
        public CategoryModel? GetById(Guid id)
        {
            return _context.Categories.Include(c => c.Movies).FirstOrDefault(c => c.Id == id);
        }
        public (bool Success, string Error, CategoryModel? Category) Create(CategoryModel category)
        {
            category.Id = Guid.NewGuid();
            category.Name = category.Name.Trim();
            category.Movies = new List<MovieModel>();

            _context.Categories.Add(category);
            _context.SaveChanges();

            return (true,string.Empty, category);
        }
        public (bool Success, string Error, CategoryModel? Category) Update(Guid Id,CategoryModel category)
        {
            var existingCategory = _context.Categories.Find(category.Id);
            if (existingCategory == null) return (false,"Kategori bulunamadı",null);
            existingCategory.Name = category.Name.Trim();
            existingCategory.Movies = category.Movies;
           
            _context.SaveChanges();
            return (true,"Kategori başarıyla güncellendi",existingCategory);
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
