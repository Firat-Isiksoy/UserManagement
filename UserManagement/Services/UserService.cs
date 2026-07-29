using Azure.Core;
using UserManagement.Models;
using UserManagement.Mappers;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserDto> GetAll() => _context.Users.Select(u => u.ToDto()).ToList();
        public UserDto? GetById(Guid id) => _context.Users.Find(id)?.ToDto();

        public ResponseModel<UserDto> Create(UserDto request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return new ResponseModel<UserDto>
                {
                    Success = false,
                    Error = "Bu e-posta adresi zaten kayıtlı",
                    Data = null
                };
            }
            var userModel = request.ToModel();
            userModel.Id = Guid.NewGuid(); 

            _context.Users.Add(userModel);
            _context.SaveChanges();
         
            return new ResponseModel<UserDto>
            {
                Success = true,
                Error = null,
                Data = userModel.ToDto()
            };
        }
        public ResponseModel<UserDto> Update(Guid id, UserDto request)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
            {
                return new ResponseModel<UserDto>
                {
                    Success = false,
                    Error = "Kullanıcı bulunamadı",
                    Data = null
                };
            }
            if (_context.Users.Any(u => u.Email == request.Email && u.Id != id))
            {
                return new ResponseModel<UserDto>
                {
                    Success = false,
                    Error = "Bu e-posta başka birine ait",
                    Data = null
                };
            }
            request.UpdateModel(existingUser);
            _context.SaveChanges();
           
            return new ResponseModel<UserDto>
            {
                Success = true,
                Error = null,
                Data = existingUser.ToDto()
            };
        }
        public bool Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user is null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
