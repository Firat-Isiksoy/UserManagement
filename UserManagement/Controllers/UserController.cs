using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

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
            FirstName = request.FirstName.Trim().ToLower(),
            LastName = request.LastName.Trim().ToLower(),
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
    public IActionResult Update(Guid id, UserDto request)
    {
        var userModel = new UserModel
        {
            Id = id,
            FirstName = request.FirstName.Trim().ToLower(),
            LastName = request.LastName.Trim().ToLower(),
            Email = request.Email
        };

        var (success, error, updatedUser) = _userService.Update(id,userModel);

        if (!success)
        {
            return BadRequest(error);
        }
        var response = new UserDto
        {
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName,
            Email = updatedUser.Email
        };
        return Ok(new
        {
            Message = "Kullanıcı başarıyla güncellendi",
            Data = response
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _userService.Delete(id);
        return deleted ? Ok("Kullanıcı başarıyla silindi") : NotFound("Kullanıcı bulunamadı.");
    }
}