using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create(CategoryModel categoryModel)
        {
            var (success, error) = _categoryService.Create(categoryModel);
            return success ? Ok("Kategori başarıyla eklendi") : BadRequest(error);
        }

    }
}
