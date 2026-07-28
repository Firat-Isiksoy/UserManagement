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

        public (bool Success, string Error, UserModel? User) Create(UserModel user)
        {
           user.Id = Guid.NewGuid();
           user.FirstName = user.FirstName?.Trim();
           user.LastName = user.LastName?.Trim();
           user.Email = user.Email?.Trim().ToLower();

           _context.Users.Add(user);
           _context.SaveChanges();

           return (true,string.Empty, user);
        }
        public (bool Success, string Error, UserModel? User) Update(Guid id, UserModel updatedUser)
        {
            var user = _context.Users.Find(id);
            if (_context.Users.Any(u => u.Email == updatedUser.Email && u.Id != id))
                return (false, "Bu e-posta adresi zaten var.",null);
            if (user is null)
                return (false, "Kullanıcı bulunamadı.",null);

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return (true, string.Empty,user);
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
