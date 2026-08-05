using Azure.Core; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers { 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult GetAllUsers() => Ok(_userService.GetAll());
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _userService.GetById(id);
            return user is null ? NotFound("Kullanıcı bulunamadı.") : Ok(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] UserCreateDto request)
        {
            var response = _userService.Create(request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id, [FromBody] UserCreateDto request)
        {
            var response = _userService.Update(id, request);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var deleted = _userService.Delete(id);
            return deleted ? Ok("Kullanıcı başarıyla silindi") : NotFound("Kullanıcı bulunamadı.");
        }
    }
}