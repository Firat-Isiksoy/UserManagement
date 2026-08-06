using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto request)
        {
            var response = _authService.Login(request);
            if (!response.Success) return Unauthorized(response);
            return Ok(response);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserCreateDto request)
        {
            var response = _authService.Register(request);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("update-profile")]
        [Authorize]
        public IActionResult UpdateProfile([FromBody] UserCreateDto request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();
            var userId = Guid.Parse(userIdString);

            var response = _authService.UpdateProfile(userId, request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpDelete("delete-account")]
        [Authorize] 
        public IActionResult DeleteMyAccount()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdString);
            var isDeleted = _authService.DeleteAccount(userId);
            if (!isDeleted)
            {
                return NotFound(new { Message = "Hesap bulunamadı veya zaten silinmiş." });
            }
            return Ok(new { Message = "Hesabınız başarıyla silinmiştir. Elveda!" });
        }
    }
}