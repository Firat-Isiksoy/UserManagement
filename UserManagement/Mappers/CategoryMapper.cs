using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(this CategoryModel category)
        {
            if (category == null) return null;
            return new CategoryDto
            {
          
                Name = category.Name,
            };
        }
        public static CategoryModel ToModel(this CategoryDto dto)
        {
            if (dto == null) return null;
            return new CategoryModel
            {
                Name = dto.Name,
            };
        }
        public static void UpdateModel(this CategoryDto dto, CategoryModel existingCategory)
        {
            if (dto == null || existingCategory == null) return;        
            existingCategory.Name = dto.Name.Trim();
        }
    }
}
