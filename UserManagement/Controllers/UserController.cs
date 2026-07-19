using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
           
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult Create(UserModel userModel)
        {
            bool emailKullanimdaMi = _context.Users.Any(u => u.Email == userModel.Email);
            if (emailKullanimdaMi)
            {
                return BadRequest("Bu e-posta adresi zaten var.");
            }
            userModel.Id = Guid.NewGuid(); 
            userModel.CreatedAt = DateTime.UtcNow;
            userModel.UpdatedAt = DateTime.UtcNow; 
           
            _context.Users.Add(userModel);
            _context.SaveChanges();
            return Ok("Kullanıcı başarıyla eklendi");

        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, UserModel updateduser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            user.FirstName = updateduser.FirstName;
            user.LastName = updateduser.LastName;
            user.Email = updateduser.Email;
            user.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok("Kullanıcı başarıyla güncellendi");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok("Kullanıcı başarıyla silindi");
        }
       
    }
}
