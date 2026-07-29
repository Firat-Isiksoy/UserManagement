using Azure.Core;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserModel> GetAll() => _context.Users.ToList();

        public UserModel? GetById(Guid id) => _context.Users.Find(id);

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
            var userModel = new UserModel()
            {
                FirstName = request.FirstName.Trim().ToLower(),
                LastName = request.LastName.Trim().ToLower(),
                Email = request.Email?.Trim().ToLower(),
            };

            _context.Users.Add(userModel);
            _context.SaveChanges();
            var userDto = new UserDto()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email
            };

            return new ResponseModel<UserDto>
            {
                Success = true,
                Error = null,
                Data = userDto
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
            if (_context.Users.Any(u => u.Email == existingUser.Email && u.Id != id))
            {
                return new ResponseModel<UserDto>
                {
                    Success = false,
                    Error = "Bu e-posta başka birine ait",
                    Data = null
                };
            }
           var updatedUser = new UserModel
            {
                FirstName = request.FirstName.Trim().ToLower(),
                LastName = request.LastName.Trim().ToLower(),
                Email = request.Email?.Trim().ToLower(),
            };
            _context.Users.Update(updatedUser);
            _context.SaveChanges();
            var responseDto = new UserDto
            {
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email
            };
            return new ResponseModel<UserDto>
            {
                Success = true,
                Error = null,
                Data = responseDto
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
