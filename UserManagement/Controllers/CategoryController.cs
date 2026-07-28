using Azure.Core;
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
                Name = request.Name.Trim().ToLower()
            };

            var (success, error, createdCategory) = _categoryService.Create(categoryModel);

            if (!success)
            {
                return BadRequest(error);
            }
            var response = new CategoryDto
            {
                Name = createdCategory.Name,
            };
            return Ok(new
            {
                Message = "Kategori başarıyla eklendi",
                Data = response
            });
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CategoryDto request)
        {
            var categoryModel = new CategoryModel
            {
                Id = id,
                Name = request.Name.Trim().ToLower()
            };
            
                var (success, error,updatedCategory) = _categoryService.Update(id,categoryModel);

                if (!success)
                {
                    return BadRequest(error);
                }
                var response = new CategoryDto
                {
                    Name = updatedCategory.Name,
                };
                return Ok(new
                {
                    Message = "Kategori başarıyla güncellendi",
                    Data = response
                });

        }
            [HttpDelete("{id}")]
            public IActionResult Delete(Guid id)
            {
                var deleted = _categoryService.Delete(id);
                return deleted ? Ok("Kategori başarıyla silindi") : NotFound("Kategori bulunamadı.");

            } 
    }
}

