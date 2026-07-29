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
        var  response = _userService.Create(request);
        if (!response.Success) return BadRequest(response.Error);
        return Ok(new { Message = "Kullanıcı başarıyla eklendi", response.Data });
    }
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UserDto request)
    {      
        var response = _userService.Update(id,request);
        if (!response.Success)
        {
            return BadRequest(response.Error);
        }    
        return Ok(new
        { Message = "Kullanıcı başarıyla güncellendi", response.Data});
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _userService.Delete(id);
        return deleted ? Ok("Kullanıcı başarıyla silindi") : NotFound("Kullanıcı bulunamadı.");
    }
}