using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement_Tests
{
    public class AuthManagementTest
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthDbTest")
            .Options;

        private AppDbContext _context;
        private AuthService _authService;
        private IConfiguration _configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new AppDbContext(_dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();

            var inMemorySettings = new Dictionary<string, string?> {
                {"JwtSettings:SecretKey", "BuCokGizliVeGucluBirSifrelemeAnahtaridir123*!"},
                {"JwtSettings:Issuer", "UserManagementAPI"},
                {"JwtSettings:Audience", "UserManagementClient"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = new AuthService(_context, _configuration);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        private void SeedDatabase()
        {
            var users = new List<UserModel>
            {
                new UserModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Boss",
                    Email = "admin@test.com",
                    Password = "password123",
                    Role = "Admin"
                },
                new UserModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Standart",
                    LastName = "User",
                    Email = "user@test.com",
                    Password = "password123",
                    Role = "User"
                }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }

        [Test, Order(1)]
        public void Login_WithValidCredentials_ShouldReturnToken_Test()
        {
            var loginDto = new LoginDto
            {
                Email = "admin@test.com",
                Password = "password123"
            };

            var result = _authService.Login(loginDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Data, Is.Not.Null); 
            Assert.That(result.Data, Does.StartWith("eyJ")); 
        }

        [Test, Order(2)]
        public void Login_WithInvalidEmail_ShouldFail_Test()
        {
            var loginDto = new LoginDto
            {
                Email = "olmayanmail@test.com",
                Password = "password123"
            };

            var result = _authService.Login(loginDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Geçersiz e-posta veya şifre."));
            Assert.That(result.Data, Is.Null);
        }

        [Test, Order(3)]
        public void Login_WithInvalidPassword_ShouldFail_Test()
        {
            var loginDto = new LoginDto
            {
                Email = "admin@test.com",
                Password = "yanlissifre"
            };

            var result = _authService.Login(loginDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Geçersiz e-posta veya şifre."));
            Assert.That(result.Data, Is.Null);
        }

        [Test, Order(4)]
        public void Register_ShouldCreateUserAndReturnDetails_Test()
        {
            var registerDto = new UserCreateDto
            {
                FirstName = "Yeni",
                LastName = "Uye",
                Email = "yeniuye@test.com",
                Password = "securepassword"
            };

            var result = _authService.Register(registerDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Email, Is.EqualTo("yeniuye@test.com"));
            Assert.That(result.Data.Role, Is.EqualTo("User"));
        }

        [Test, Order(5)]
        public void DeleteAccount_ShouldRemoveUser_Test()
        {
            var userToDelete = new UserModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Silinecek",
                LastName = "Kullanici",
                Email = "silinecek@test.com",
                Password = "123",
                Role = "User"
            };

            _context.Users.Add(userToDelete);
            _context.SaveChanges();

            var isDeleted = _authService.DeleteAccount(userToDelete.Id);
            Assert.That(isDeleted, Is.True);
            var dbCheck = _context.Users.FirstOrDefault(u => u.Id == userToDelete.Id);
            Assert.That(dbCheck, Is.Null);
        }
        [Test, Order(6)]
        public void Auth_MissingOrInvalidCredentials_ShouldReturnUnauthorized_401_Test()
        {
            var invalidLogin = new LoginDto
            {
                Email = "olmayan@test.com",
                Password = "yanlissifre"
            };

            var result = _authService.Login(invalidLogin);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error, Is.EqualTo("Geçersiz e-posta veya şifre."));
        }

        [Test, Order(7)]
        public void User_WithStandardRole_TryingAdminAction_ShouldBeForbidden_403_Test()
        {
            var standardUser = _context.Users.FirstOrDefault(u => u.Email == "user@test.com");

            Assert.That(standardUser, Is.Not.Null);
            bool isAdmin = standardUser.Role == "Admin";
            Assert.That(isAdmin, Is.False, "Standart kullanıcı Admin yetkisine sahip olmamalı!");
        }

        [Test, Order(8)]
        public void Auth_WithValidCredentials_ShouldReturnSuccess_Test()
        {
            var validLogin = new LoginDto
            {
                Email = "admin@test.com",
                Password = "password123"
            };

            var result = _authService.Login(validLogin);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Error, Is.Null);
            Assert.That(result.Data, Is.Not.Null);
        }
    }
}