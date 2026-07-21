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

        public (bool Success, string Error) Create(UserModel user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                return (false, "Bu e-posta adresi zaten var.");

            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            _context.SaveChanges();
            return (true, string.Empty);
        }

        public (bool Success, string Error) Update(Guid id, UserModel updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user is null)
                return (false, "Kullanıcı bulunamadı.");

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return (true, string.Empty);
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
