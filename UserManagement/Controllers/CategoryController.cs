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
            var response = _categoryService.Create(request);

            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(new { Message = "Kategori başarıyla eklendi", response.Data });
        }     
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, CategoryDto request)
        {            
            var response = _categoryService.Update(id,request);
               if (!response.Success)
               {
                   return BadRequest(response.Error);
               }
               return Ok(new { Message = "Kategori başarıyla güncellendi", response.Data });
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
             var deleted = _categoryService.Delete(id);
             return deleted ? Ok("Kategori başarıyla silindi") : NotFound("Kategori bulunamadı.");
        } 
    }
}

