using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllCategories() => Ok(_categoryService.GetAll());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(Guid id)
        {
            var category = _categoryService.GetById(id);
            return category is null ? NotFound("Kategori bulunamadı.") : Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CategoryDto request)
        {
            var response = _categoryService.Create(request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id, [FromBody] CategoryDto request)
        {
            var response = _categoryService.Update(id, request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _categoryService.Delete(id);
            return deleted ? Ok("Kategori başarıyla silindi") : NotFound("Kategori bulunamadı.");
        }
    }
}