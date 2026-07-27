using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.DTOs;

[Route("api/[controller]")]
[ApiController]
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
    public IActionResult Create(UserDto request)
    {
        var userModel = new UserModel
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        var (success, error, createdUser) = _userService.Create(userModel);

        if (!success)
        {
            return BadRequest(error);
        }
        var response = new UserDto
        {
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Email = createdUser.Email
        };
        return Ok(new
        {
            Message = "Kullanıcı başarıyla eklendi",
            Data = response
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UserModel updatedUser)
    {
        var (success, error) = _userService.Update(id, updatedUser);
        return success ? Ok("Kullanıcı başarıyla güncellendi") : NotFound(error);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _userService.Delete(id);
        return deleted ? Ok("Kullanıcı başarıyla silindi") : NotFound("Kullanıcı bulunamadı.");
    }
}