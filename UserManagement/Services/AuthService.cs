using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagement.DTOs;
using UserManagement.Mappers;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public ResponseModel<string> Login(LoginDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Success = false,
                    Error = "Geçersiz e-posta veya şifre.",
                    Data = null
                };
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };            
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);           
            return new ResponseModel<string>
            {
                Success = true,
                Error = null,
                Data = tokenString
            };
        }
        public ResponseModel<UserDetailsDto> Register(UserCreateDto request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return new ResponseModel<UserDetailsDto>
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

            return new ResponseModel<UserDetailsDto>
            {
                Success = true,
                Error = null,
                Data = userModel.ToDetailsDto()
            };
        }
        public ResponseModel<UserDetailsDto> UpdateProfile(Guid userId, UserCreateDto request)
        {
            var existingUser = _context.Users.Find(userId);

            if (existingUser == null)
            {
                return new ResponseModel<UserDetailsDto>
                {
                    Success = false,
                    Error = "Kullanıcı bulunamadı",
                    Data = null
                };
            }
            if (_context.Users.Any(u => u.Email == request.Email && u.Id != userId))
            {
                return new ResponseModel<UserDetailsDto>
                {
                    Success = false,
                    Error = "Bu e-posta başka birine ait",
                    Data = null
                };
            }
            request.UpdateModel(existingUser);
            _context.SaveChanges();
            return new ResponseModel<UserDetailsDto>
            {
                Success = true,
                Error = null,
                Data = existingUser.ToDetailsDto()
            };
        }
        public bool DeleteAccount(Guid userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}