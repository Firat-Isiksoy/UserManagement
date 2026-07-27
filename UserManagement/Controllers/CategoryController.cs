using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetAllCategories() => Ok(_categoryService.GetAll());
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var category = _categoryService.GetById(id);
            return category is null ? NotFound("Kategori bulunamadı.") : Ok(category);
        }
        [HttpPost]
        public IActionResult Create(CategoryDto request)
        {
            var categoryModel = new CategoryModel
            {
                Name = request.Name
            };

            var (success, error, createdCategory) = _categoryService.Create(categoryModel);

            if (!success)
            {
                return BadRequest(error);
            }
            var response = new CategoryDto
            {
                Name = createdCategory.Name,
                Movies = createdCategory.Movies
            };
            return Ok(new
            {
                Message = "Kategori başarıyla eklendi",
                Data = response
            });
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CategoryModel updatedCategory)
        {
            var (success, error) = _categoryService.Update(id, updatedCategory);
            return success ? Ok("Kategori başarıyla güncellendi") : NotFound(error);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id, CategoryModel updatedCategory)
        {
            var deleted = _categoryService.Delete(id);
            return deleted ? Ok("Kategori başarıyla silindi") : NotFound("Kategori bulunamadı.");

        }

    }
}
