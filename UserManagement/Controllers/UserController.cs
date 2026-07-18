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
        [HttpGet]
        public new ResponseModel GetAllUsers()
        {
            return new ResponseModel()
            {
                Message = "Liste başarıyla getirildi"
            };
        }
        [HttpGet("{id}")]
        public new ResponseModel Get(int id)
        {
            return new ResponseModel()
            {
                Message = "Kullanıcı bulundu"
            };
        }
        [HttpPost]
        public new ResponseModel Create(UserModel UserModel)
        {
            return new ResponseModel()
            {
                Message = "Kullanıcı başarıyla oluşturuldu"
            };

        }
        [HttpPut("{id}")]
        public new ResponseModel Update(int id,UserModel updateduser)
        {
            
            return new ResponseModel()
            {
                Message = "Kullanıcı başarıyla güncellendi"
            };
        }
        [HttpDelete("{id}")]
        public new ResponseModel Delete(int id)
        {
            return new ResponseModel()
            {
                Message = "Kullanıcı başarıyla silindi"
            };
        }
       
    }
}
